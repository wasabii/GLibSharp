using System.IO;
using System.Xml.Linq;
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
            var builder = provider.GetRequiredService<SyntaxBuilderFactory>().Create(syntax);

            var glib = File.ReadAllText("GLib-2.0.gir");
            var xslt = File.ReadAllText("GLib-2.0.gir.xslt");
            var temp = new XDocument();

            var xsl = new System.Xml.Xsl.XslCompiledTransform();
            xsl.Load(XDocument.Parse(xslt).CreateReader());

            using (var rdr = XDocument.Parse(glib).CreateReader())
            using (var wrt = temp.CreateWriter())
                xsl.Transform(rdr, wrt);

            // add repositories to be built
            var repositories = new RepositoryXmlSource();
            repositories.Load("fontconfig-2.0.gir");
            repositories.Load(temp);
            repositories.Load("GObject-2.0.gir");
            repositories.Load("Gio-2.0.gir");
            builder.AddSource(repositories);
            builder.AddNamespace("GLib");
            builder.AddNamespace("Gio");

            using (var wrt = new StringWriter())
            {
                wrt.Write(builder.Build().NormalizeWhitespace().ToFullString());
                wrt.Flush();

                var str = wrt.ToString();
            }
        }

    }

}
