using System;
using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

using Autofac;

using GObject.Introspection.CodeGen;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.Editing;

namespace GObject.Introspection.Tools
{

    /// <summary>
    /// Handles the 'codegen' command.
    /// </summary>
    class CodeGen : ICommandHandler
    {

        /// <summary>
        /// Arguments to pass to the codegen command.
        /// </summary>
        class Args
        {

            /// <summary>
            /// Modules to import services from.
            /// </summary>
            public string[] Modules { get; set; }

            /// <summary>
            /// Repositories to import.
            /// </summary>
            public string[] Imports { get; set; }

            /// <summary>
            /// Namespaces to export.
            /// </summary>
            public string[] Exports { get; set; }

            /// <summary>
            /// Code language to generate.
            /// </summary>
            public string Lang { get; set; }

            /// <summary>
            /// Version of code language to generate.
            /// </summary>
            public double? LangVersion { get; set; }

            /// <summary>
            /// File to output generated code into.
            /// </summary>
            public string Output { get; set; }

        }

        /// <summary>
        /// Attempts to load the referenced assemblies.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Assembly> GetModuleAssemblies(string[] modules)
        {
            foreach (var module in modules)
            {
                var f = new FileInfo(module);
                if (f.Exists)
                    if (TryLoadAssembly(f) is Assembly a)
                        yield return a;
            }
        }

        /// <summary>
        /// Tries to load the specified assembly.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        Assembly TryLoadAssembly(FileInfo file)
        {
            try
            {
                return Assembly.LoadFrom(file.FullName);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Runs the command.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task<int> InvokeAsync(InvocationContext context)
        {
            // extract options
            var args = new Args()
            {
                Modules = context.ParseResult.ValueForOption<string[]>("module"),
                Imports = context.ParseResult.ValueForOption<string[]>("import"),
                Exports = context.ParseResult.ValueForOption<string[]>("export"),
                Lang = context.ParseResult.ValueForOption<string>("lang"),
                LangVersion = context.ParseResult.ValueForOption<double?>("lang-version"),
                Output = context.ParseResult.ValueForOption<string>("output"),
            };

            // build container
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterAssemblyModules(typeof(GObject.Introspection.CodeGen.TypeInfo).Assembly);
            containerBuilder.RegisterAssemblyModules(GetModuleAssemblies(args.Modules).ToArray());

            // generate service provider
            var container = containerBuilder.Build();

            // build workspace for code generation
            var workspace = new AdhocWorkspace();
            workspace.Options.WithChangedOption(CSharpFormattingOptions.IndentBraces, true);

            // build generator and builder for transforming
            var syntax = SyntaxGenerator.GetGenerator(workspace, args.Lang ?? LanguageNames.CSharp);
            var builder = container.Resolve<SyntaxBuilderFactory>().Create(syntax);

            // set of source GIR files
            var repositories = new RepositoryXmlSource();

            // parse specified repositories
            foreach (var import in args.Imports.Distinct())
            {
                // pull out paths
                var s = import.Split(',');
                var xmlPath = s.Length > 0 ? s[0]?.Trim().Trim('"') : null;
                var xslPath = s.Length > 1 ? s[1]?.Trim().Trim('"') : null;

                // load GIR file
                if (string.IsNullOrWhiteSpace(xmlPath) || File.Exists(xmlPath) == false)
                    throw new FileNotFoundException($"Missing XML GIR file: '{xmlPath}'", xmlPath);

                // parse GIR file
                var xmlData = XDocument.Parse(File.ReadAllText(xmlPath));

                // a transform was specified
                if (string.IsNullOrWhiteSpace(xslPath) == false)
                {
                    if (File.Exists(xslPath) == false)
                        throw new FileNotFoundException($"Missing XSLT GIR transform file: '{xslPath}'.", xslPath);

                    // load transform from path
                    var xfr = new System.Xml.Xsl.XslCompiledTransform();
                    using (var xslRdr = File.OpenRead(xslPath))
                    using (var xslXml = XmlReader.Create(xslRdr))
                        xfr.Load(xslXml);

                    // transform into temporary document
                    var tmp = new XDocument();
                    using (var rdr = xmlData.CreateReader())
                    using (var wrt = tmp.CreateWriter())
                        xfr.Transform(rdr, wrt);

                    // update XML with transformed
                    xmlData = tmp;
                }

                // add final data to repository
                repositories.Load(xmlData);
            }

            // add repositorys to builder
            builder.AddSource(repositories);

            // add namespaces to be exported
            foreach (var export in args.Exports.Distinct())
                if (string.IsNullOrWhiteSpace(export) == false)
                    builder.AddExport(export);

            // delete existing file
            if (File.Exists(args.Output))
                File.Delete(args.Output);

            // generate file into output
            using (var stm = File.OpenWrite(args.Output))
            using (var wrt = new StreamWriter(stm))
            {
                try
                {
                    // export the configured namespaces
                    var rsl = builder.Export();

                    // output any log messages
                    foreach (var message in rsl.Messages)
                    {
                        switch (message.Severity)
                        {
                            case SyntaxBuilderMessageSeverity.Error:
                            case SyntaxBuilderMessageSeverity.Warning:
                                context.Console.Error.Write(message.ToString());
                                context.Console.Error.Write("\n");
                                break;
                            default:
                                context.Console.Out.Write(message.ToString());
                                context.Console.Out.Write("\n");
                                break;
                        }
                    }

                    // exit if no node generated
                    if (rsl.Node == null)
                        return Task.FromResult(1);

                    // clean up the code and output to file
                    wrt.Write(rsl.Node.NormalizeWhitespace().ToFullString());
                }
                catch (SyntaxBuilderException e)
                {
                    context.Console.Error.Write($"{e.Context.DebugText}\n{e}");
                    return Task.FromResult(1);
                }
            }

            return Task.FromResult(0);
        }

    }

}
