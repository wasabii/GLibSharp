using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

using Gir.CodeGen;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.Extensions.DependencyInjection;

namespace Gir.Sdk
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
        public ITaskItem[] References { get; set; }

        /// <summary>
        /// GIR files to be imported.
        /// </summary>
        public ITaskItem[] Repositories { get; set; }

        /// <summary>
        /// Namespaces to be generated.
        /// </summary>
        public ITaskItem[] Namespaces { get; set; }

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
            foreach (var i in References)
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
            foreach (var repository in Repositories)
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
            foreach (var ns in Namespaces)
                if (string.IsNullOrWhiteSpace(ns.ItemSpec) == false)
                    builder.AddNamespace(ns.ItemSpec);

            // generate file into output
            using (var stm = File.OpenWrite(Output))
            using (var wrt = new StreamWriter(stm))
                wrt.Write(builder.Build().NormalizeWhitespace().ToFullString());

            return true;
        }

    }

}
