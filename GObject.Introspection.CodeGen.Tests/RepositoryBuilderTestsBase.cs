using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Autofac;
using GObject.Introspection.Library;
using GObject.Introspection.Model;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.Extensions.DependencyInjection;

namespace GObject.Introspection.CodeGen.Tests
{

    public abstract class RepositoryBuilderTestsBase
    {

        /// <summary>
        /// Builds a testable GIR file with the specified namespace and content added.
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        protected XDocument BuildXml(string ns, params XElement[] content)
        {
            return new XDocument(
                new XElement(Xmlns.Core_1_0_NS + "repository",
                    new XAttribute("version", "1.2"),
                    new XElement(Xmlns.Core_1_0_NS + "namespace",
                        new XAttribute("name", "GLib"),
                        new XAttribute("version", "1.0"),
                        new XElement(Xmlns.Core_1_0_NS + "primitive",
                            new XAttribute("name", "guint"),
                            new XAttribute(Xmlns.CLR_1_0_NS + "type", "System.UInt32"))),
                    new XElement(Xmlns.Core_1_0_NS + "namespace",
                        new XAttribute("name", ns),
                        new XAttribute("version", "1.0"),
                        content)));
        }

        /// <summary>
        /// Builds a testable GIR file with a test namespace and content added.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        protected XDocument BuildXml(params XElement[] content)
        {
            return BuildXml("Test", content);
        }

        /// <summary>
        /// Builds the given namespace out of the given XML into an assembly.
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        protected Assembly ExportNamespace(XDocument xml, string name, string version)
        {
            // build workspace for code generation
            var workspace = new AdhocWorkspace();
            workspace.Options.WithChangedOption(CSharpFormattingOptions.IndentBraces, true);
            var syntax = SyntaxGenerator.GetGenerator(workspace, LanguageNames.CSharp);

            // build container
            var services = new ContainerBuilder();
            services.RegisterAssemblyTypes(typeof(SyntaxBuilder).Assembly).Where(i => i.IsAssignableTo<ISyntaxNodeBuilder>()).As<ISyntaxNodeBuilder>();
            services.RegisterType<SyntaxBuilder>().AsSelf();
            services.RegisterInstance(syntax);
            services.RegisterInstance(new NamespaceLibrary(new NamespaceXmlSource(xml));
            var container = services.Build();
            var builder = container.Resolve<SyntaxBuilder>();

            // export code and check for errors
            var rsl = builder.ExportNamespace(name, version);
            if (rsl == null)
                throw new InvalidOperationException();

            var syn = rsl.NormalizeWhitespace();

            using (var wrt = new StringWriter())
            {
                wrt.Write(syn.ToFullString());
                wrt.Flush();
                var str = wrt.ToString();
                syn = CSharpSyntaxTree.ParseText(str).GetRoot();
            }

            // begin with hard coded references
            var references = Enumerable.Empty<string>();

            // add reference assemblies
            references = references.Concat(
                Directory.GetFiles(Path.Combine(Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName, "refs")));

            // ensure GIR project is referenced
            references = references.Append(typeof(GObject.Introspection.TypeName).Assembly.Location);

            // finalize list
            references = references
                .Where(i => File.Exists(i))
                .Distinct()
                .ToList();

            var compilation = CSharpCompilation.Create(
                "TestAssembly",
                new[] { SyntaxFactory.SyntaxTree(syn) },
                references.Select(i => MetadataReference.CreateFromFile(i)),
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            var stm = new MemoryStream();
            var emt = compilation.Emit(stm);
            if (emt.Success == false)
                throw new Exception(string.Join("\n", emt.Diagnostics));

            var asm = Assembly.Load(stm.ToArray());

            return asm;
        }

        /// <summary>
        /// Builds the Test namespace out of the given XML into an assembly.
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        protected Assembly Build(XDocument xml)
        {
            return Build(xml, "Test");
        }

        /// <summary>
        /// Builds the Test namespace out of the given XML into an assembly.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        protected Assembly Build(params XElement[] content)
        {
            return Build(BuildXml(content), "Test");
        }

    }

}
