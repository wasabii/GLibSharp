using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
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

            // add repositories to be built
            var repositories = new RepositoryXmlSource();
            repositories.Load(XDocument.Parse(File.ReadAllText("Test-1.0.gir")));
            builder.AddSource(repositories);
            builder.AddNamespace("Test");

            var syn = builder.Build().NormalizeWhitespace();

            using (var wrt = new StringWriter())
            {
                wrt.Write(syn.ToFullString());
                wrt.Flush();
                var str = wrt.ToString();
            }

            var compilation = CSharpCompilation.Create("test",
                new[] { SyntaxFactory.SyntaxTree(syn) },
                AppDomain.CurrentDomain.GetAssemblies()
                    .Where(i => i.IsDynamic == false && File.Exists(i.Location))
                    .Select(i => MetadataReference.CreateFromFile(i.Location))
                    .Append(MetadataReference.CreateFromFile(typeof(Gir.TypeName).GetTypeInfo().Assembly.Location)),
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            var stm = new MemoryStream();
            var rsl = compilation.Emit(stm);
            if (rsl.Success == false)
                foreach (var i in rsl.Diagnostics)
                    Console.Write(i.ToString());

            var asm = Assembly.Load(stm.ToArray());
        }

    }

}
