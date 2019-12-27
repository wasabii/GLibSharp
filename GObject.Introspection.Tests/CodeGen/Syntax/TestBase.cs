using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using GObject.Introspection.CodeGen.Model;
using GObject.Introspection.CodeGen.Syntax;
using GObject.Introspection.Library;
using GObject.Introspection.Library.Model;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.Editing;

namespace GObject.Introspection.Tests.CodeGen.Syntax
{

    public abstract class TestBase
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

            // finalize list
            var directory = Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName;
            var metadata = Directory.GetFiles(Path.Combine(directory, "refs"))
                .Distinct()
                .Where(i => File.Exists(i))
                .Select(i => MetadataReference.CreateFromFile(i))
                .Append(MetadataReference.CreateFromFile(Path.Combine(directory, "GLib.Interop.dll")));

            // begin generating a DLL
            var csharp = CSharpCompilation.Create("GInterop." + name, null, metadata, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            var syntax = SyntaxGenerator.GetGenerator(workspace, csharp.Language);

            // begin namespace library
            var namespaces = new NamespaceLibrary(new NamespaceXmlSource(xml));

            // begin build library
            var assemblies = csharp.References.Select(i => csharp.GetAssemblyOrModuleSymbol(i)).OfType<IAssemblySymbol>();
            var modules = new ModuleLibrary(namespaces, new MetadataTypeResolver(assemblies));

            // begin module builder
            var builder = new ModuleBuilder(modules.ResolveModule(name, version), syntax);

            // export code and check for errors
            var rsl = syntax.CompilationUnit(builder.Build());
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
            csharp = csharp.AddSyntaxTrees(SyntaxFactory.SyntaxTree(syn));

            var stm = new MemoryStream();
            var emt = csharp.Emit(stm);
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
