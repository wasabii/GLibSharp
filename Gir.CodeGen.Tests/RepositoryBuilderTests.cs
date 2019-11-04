using System.IO;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gir.CodeGen.Tests
{

    [TestClass]
    public class RepositoryBuilderTests
    {

        [TestMethod]
        public void TestMethod1()
        {
            // build container
            var services = new ServiceCollection();
            services.AddGirCodeGen();
            var provider = services.BuildServiceProvider();

            // build workspace for code generation
            var workspace = new AdhocWorkspace();
            workspace.Options.WithChangedOption(CSharpFormattingOptions.IndentBraces, true);

            // build generator and builder for transforming
            var syntax = SyntaxGenerator.GetGenerator(workspace, LanguageNames.CSharp);
            var builder = provider.GetRequiredService<RepositoryBuilderFactory>().Create(syntax);

            // add repositories to be built
            var repositories = new SymbolXmlSource();
            repositories.Load("fontconfig-2.0.gir");
            repositories.Load("GLib-2.0.gir");
            repositories.Load("Gio-2.0.gir");
            builder.AddSource(repositories);
            builder.AddNamespace("GLib", "2.0");
            builder.AddNamespace("Gio", "2.0");

            using (var wrt = new StringWriter())
            {
                wrt.Write(builder.Build().NormalizeWhitespace().ToFullString());
                wrt.Flush();

                var str = wrt.ToString();
            }
        }

    }

}
