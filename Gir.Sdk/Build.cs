using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

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
        public string Lang { get; set; } = "C#";

        /// <summary>
        /// Target compilation language version.
        /// </summary>
        public string LangVersion { get; set; } = "7.0";

        /// <summary>
        /// Set of assemblies to search for building modules.
        /// </summary>
        public ITaskItem[] ModuleAssemblies { get; set; }

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
            foreach (var i in ModuleAssemblies)
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

            // register plugins
            foreach (var asm in GetModuleAssemblies())
                services.AddGirCodeGen(asm);

            // provides access to the services
            var provider = services.BuildServiceProvider();

            // build workspace for code generation
            var workspace = new AdhocWorkspace();
            workspace.Options.WithChangedOption(CSharpFormattingOptions.IndentBraces, true);

            // create syntax builder instance
            var builder = provider
                .GetRequiredService<SyntaxBuilderFactory>()
                .Create(SyntaxGenerator.GetGenerator(workspace, Lang));

            // set of source GIR files
            var repositories = new RepositoryXmlSource();
            foreach (var gir in Repositories)
                repositories.Load(gir.GetMetadata("FullPath") ?? gir.ItemSpec);
            builder.AddSource(repositories);

            // add namespaces to be built
            foreach (var ns in Namespaces)
                builder.AddNamespace(ns.ItemSpec);

            // generate file into output
            using (var stm = File.OpenWrite(Output))
            using (var wrt = new StreamWriter(stm))
                wrt.Write(builder.Build().NormalizeWhitespace().ToFullString());

            return true;
        }

    }

}
