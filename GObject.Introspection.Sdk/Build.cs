using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

using GObject.Introspection.CodeGen;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.Editing;

namespace GObject.Introspection.Sdk
{

    public class Build : Task
    {

        /// <summary>
        /// Target compilation language.
        /// </summary>
        public string Language { get; set; } = "C#";

        /// <summary>
        /// Target compilation language version.
        /// </summary>
        public string LanguageVersion { get; set; } = "7.0";

        /// <summary>
        /// Set of assemblies to search for building modules.
        /// </summary>
        public ITaskItem[] Module { get; set; }

        /// <summary>
        /// GIR files to be imported.
        /// </summary>
        public ITaskItem[] Import { get; set; }

        /// <summary>
        /// Namespaces to be generated.
        /// </summary>
        public ITaskItem[] Export { get; set; }

        /// <summary>
        /// Target of the output.
        /// </summary>
        public string Output { get; set; }

        /// <summary>
        /// Attempts to load the referenced assemblies.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Assembly> GetModuleAssemblies()
        {
            foreach (var i in Module)
            {
                var f = new FileInfo(i.ItemSpec);
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
            catch (Exception e)
            {
                Log.LogErrorFromException(e);
                return null;
            }
        }

        public override bool Execute()
        {
            // build container
            var services = new ServiceCollection();
            services.AddGirCodeGen();
            var provider = services.BuildServiceProvider();

            //// register plugins
            //foreach (var asm in GetModuleAssemblies())
            //    services.AddGirCodeGen(asm);

            // build workspace for code generation
            var workspace = new AdhocWorkspace();
            workspace.Options.WithChangedOption(CSharpFormattingOptions.IndentBraces, true);

            // build generator and builder for transforming
            var syntax = SyntaxGenerator.GetGenerator(workspace, LanguageNames.CSharp);
            var builder = provider.GetRequiredService<SyntaxBuilderFactory>().Create(syntax);

            // set of source GIR files
            var repositories = new RepositoryXmlSource();

            // parse specified repositories
            foreach (var repository in Import)
            {
                // load GIR file
                var xmlPath = repository.GetMetadata("FullPath")?.Trim() ?? repository.ItemSpec?.Trim();
                if (string.IsNullOrWhiteSpace(xmlPath) || File.Exists(xmlPath) == false)
                    throw new FileNotFoundException($"Missing XML GIR file: '{xmlPath}'", xmlPath);

                // parse GIR file
                var xmlData = XDocument.Parse(File.ReadAllText(xmlPath));

                // a transform was specified
                var xslPath = repository.GetMetadata("XsltPath")?.Trim();
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

            // add namespaces to be built
            foreach (var ns in Export)
                if (string.IsNullOrWhiteSpace(ns.ItemSpec) == false)
                    builder.AddExport(ns.ItemSpec);

            // generate file into output
            using (var stm = File.OpenWrite(Output))
            using (var wrt = new StreamWriter(stm))
            {
                // export the configured namespaces
                var rsl = builder.Export();
                if (rsl.Node == null)
                    throw new Exception(string.Join("\n", rsl.Messages.Select(i => $"{i.Severity}: {i.Text}")));

                // output any log messages
                foreach (var message in rsl.Messages)
                {
                    switch (message.Severity)
                    {
                        case SyntaxBuilderMessageSeverity.Error:
                            Log.LogError(message.Text.Format, message.Text.GetArguments());
                            break;
                        case SyntaxBuilderMessageSeverity.Warning:
                            Log.LogWarning(message.Text.Format, message.Text.GetArguments());
                            break;
                        case SyntaxBuilderMessageSeverity.Debug:
                        default:
                            Log.LogMessage(message.Text.Format, message.Text.GetArguments());
                            break;
                    }
                }

                // clean up the code and output to file
                wrt.Write(rsl.Node.NormalizeWhitespace().ToFullString());
            }

            return true;
        }

    }

}
