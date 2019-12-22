using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

using GObject.Introspection.Library;
using GObject.Introspection.Model;
using GObject.Introspection.Reflection;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace GObject.Introspection.Tests
{

    public abstract class IntrospectionLibraryTestsBase
    {

        /// <summary>
        /// Wraps a <see cref="ITypeSymbol"/> as a <see cref="IManagedTypeReference"/>.
        /// </summary>
        class MetadataTypeReference : IManagedTypeReference
        {

            readonly ITypeSymbol symbol;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="symbol"></param>
            public MetadataTypeReference(ITypeSymbol symbol)
            {
                this.symbol = symbol ?? throw new ArgumentNullException(nameof(symbol));
            }

            public AssemblyName AssemblyName => new AssemblyName(symbol.ContainingAssembly.Identity.Name);

            public string Name => symbol.Name;

            public bool IsArray => false;

            public bool IsBlittable => false;

        }

        /// <summary>
        /// Searches <see cref="IAssemblySymbol"/> instances for managed types.
        /// </summary>
        class MetadataReferenceTypeResolver : IManagedTypeResolver
        {

            readonly IEnumerable<IAssemblySymbol> assemblies;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="assemblies"></param>
            public MetadataReferenceTypeResolver(IEnumerable<IAssemblySymbol> assemblies)
            {
                this.assemblies = assemblies ?? throw new ArgumentNullException(nameof(assemblies));
            }

            public IManagedTypeReference Resolve(string name)
            {
                return assemblies
                    .SelectMany(i => Resolve(i.GlobalNamespace, name))
                    .Select(i => new MetadataTypeReference(i))
                    .FirstOrDefault(i => i != null);
            }

            IEnumerable<ITypeSymbol> Resolve(INamespaceOrTypeSymbol parent, string name)
            {
                foreach (var m in parent.GetMembers().OfType<INamespaceOrTypeSymbol>())
                {
                    // check if type matches
                    if (m is ITypeSymbol t && t.CanBeReferencedByName && t.ContainingNamespace.Name + "." + t.Name == name)
                        yield return t;

                    // recurse into members of type
                    foreach (var i in Resolve(m, name))
                        yield return i;
                }
            }

        }

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
        protected IntrospectionModule ExportModule(XDocument xml, string name, string version)
        {
            // begin with hard coded references
            var references = Enumerable.Empty<string>();

            // add reference assemblies
            references = references.Concat(Directory.GetFiles(Path.Combine(Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName, "refs")));

            // finalize list
            var metadata = references
                .Where(i => File.Exists(i))
                .Distinct()
                .Select(i => MetadataReference.CreateFromFile(i))
                .ToList();

            var cs = CSharpCompilation.Create("Foo", null, metadata, null);
            var am = metadata.Select(i => cs.GetAssemblyOrModuleSymbol(i)).OfType<IAssemblySymbol>().ToList();

            var library = new IntrospectionLibrary(new NamespaceLibrary((NamespaceXmlSource)new NamespaceXmlSource(xml)), new MetadataReferenceTypeResolver(am));
            return library.ResolveModule(name, version);
        }

        /// <summary>
        /// Exports the Test namespace from the given repository content.
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        protected IntrospectionModule ExportModule(XDocument xml)
        {
            return ExportModule(xml, "Test", "1.0");
        }

        /// <summary>
        /// Exports the introspection module for the namespace content.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        protected IntrospectionModule ExportModule(params XElement[] content)
        {
            return ExportModule(BuildXml(content), "Test", "1.0");
        }

    }

}
