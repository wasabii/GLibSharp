using System.IO;

using Gir.CodeGen;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.CodeAnalysis;
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
        public ITaskItem[] BuildingModules { get; set; }

        /// <summary>
        /// GIR files to be built.
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

        public override bool Execute()
        {
            // build container
            var services = new ServiceCollection();
            services.AddGirCodeGen();
            var provider = services.BuildServiceProvider();

            // build workspace for code generation
            var workspace = new AdhocWorkspace();
            workspace.Options.WithChangedOption(CSharpFormattingOptions.IndentBraces, true);

            // build generator and builder for transforming
            var syntax = SyntaxGenerator.GetGenerator(workspace, Lang);
            var builder = provider.GetRequiredService<RepositoryBuilderFactory>().Create(syntax);

            // add repositories to be built
            var repositories = new SymbolXmlSource();
            foreach (var gir in Repositories)
                repositories.Load(gir.GetMetadata("FullPath") ?? gir.ItemSpec);
            builder.AddSource(repositories);

            // add namespaces to be built
            foreach (var ns in Namespaces)
                builder.AddNamespace(ns.ItemSpec, ns.GetMetadata("Version"));

            using (var stm = File.OpenWrite(Output))
            using (var wrt = new StreamWriter(stm))
                wrt.Write(builder.Build().NormalizeWhitespace().ToFullString());

            return true;
        }

    }

}
