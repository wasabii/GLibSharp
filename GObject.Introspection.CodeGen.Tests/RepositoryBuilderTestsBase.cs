using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

using Autofac;

using GObject.Introspection.CodeGen.Model;
using GObject.Introspection.Library;
using GObject.Introspection.Library.Model;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.Editing;

namespace GObject.Introspection.CodeGen.Syntax.Tests
{

    public abstract class RepositoryBuilderTestsBase
    {

        /// <summary>
        /// Builds a testable GIR file with the specified namespace and content added.
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        protected XDocument BuildXml(string ns, string version, params XElement[] content)
        {
            return new XDocument(
                new XElement(Xmlns.Core_1_0_NS + "repository",
                    new XAttribute("version", "1.2"),
                    new XElement(Xmlns.Core_1_0_NS + "namespace",
                        new XAttribute("name", ns),
                        new XAttribute("version", version),
                        content)));
        }

        /// <summary>
        /// Builds a testable GIR file with a test namespace and content added.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        protected XDocument BuildXml(params XElement[] content)
        {
            return BuildXml("Test", "1.0", content);
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

            // begin with hard coded references
            var references = Enumerable.Empty<string>();

            // add reference assemblies
            references = references.Concat(Directory.GetFiles(Path.Combine(Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName, "refs")));

            // finalize list
            var metadata = references
                .Distinct()
                .Where(i => File.Exists(i))
                .Select(i => MetadataReference.CreateFromFile(i))
                .ToList();

            var cs = CSharpCompilation.Create("GInterop." + name, null, metadata, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            var assemblies = metadata.Select(i => cs.GetAssemblyOrModuleSymbol(i)).OfType<IAssemblySymbol>().ToList();
            var library = new ModuleLibrary(new NamespaceLibrary((NamespaceXmlSource)new NamespaceXmlSource(xml)), new ManagedTypeResolver(assemblies));
            var builder = new SyntaxModuleBuilder(library, syntax);

            // export code and check for errors
            var rsl = builder.BuildModule(name, version);
            if (rsl == null)
                throw new InvalidOperationException();

            // extract generated syntax
            var syn = rsl.NormalizeWhitespace();

            // rebuild as string and back again, because of some generation error
            using (var wrt = new StringWriter())
            {
                wrt.Write(syn.ToFullString());
                wrt.Flush();
                var str = wrt.ToString();
                syn = CSharpSyntaxTree.ParseText(str).GetRoot();
            }

            // fold generated code into compilation
            cs = cs.AddSyntaxTrees(SyntaxFactory.SyntaxTree(syn));

            var stm = new MemoryStream();
            var emt = cs.Emit(stm);
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
        protected Assembly ExportNamespace(XDocument xml)
        {
            return ExportNamespace(xml, "Test", "1.0");
        }

        /// <summary>
        /// Builds the Test namespace out of the given XML into an assembly.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        protected Assembly ExportNamespace(params XElement[] content)
        {
            return ExportNamespace(BuildXml("Test", "1.0", content), "Test", "1.0");
        }

    }

}
