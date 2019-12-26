using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace GObject.Introspection.Library.Model
{

    public class NamespaceElement : Element
    {

        public static IEnumerable<NamespaceElement> LoadFrom(RepositoryElement repository, XContainer container)
        {
            return container.Elements().Select(i => Load(repository, i)).OfType<NamespaceElement>();
        }

        public static NamespaceElement Load(RepositoryElement repository, XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "namespace" ? Populate(new NamespaceElement(repository), element) : null;
        }

        public static NamespaceElement Populate(NamespaceElement target, XElement element)
        {
            Element.Populate(target, element);
            target.Name = (string)element.Attribute("name");
            target.Version = (string)element.Attribute("version");
            target.CIdentifierPrefixes = XmlUtil.ParseStringList((string)element.Attribute(Xmlns.C_1_0_NS + "identifier-prefixes"));
            target.CSymbolPrefixes = XmlUtil.ParseStringList((string)element.Attribute(Xmlns.C_1_0_NS + "symbol-prefixes"));
            target.CPrefix = (string)element.Attribute(Xmlns.C_1_0_NS + "prefix");
            target.SharedLibraries = XmlUtil.ParseStringList((string)element.Attribute("shared-library"));
            target.ClrSharedLibrary = (string)element.Attribute(Xmlns.CLR_1_0_NS + "shared-library");
            target.Primitives = PrimitiveElement.LoadFrom(element).ToList();
            target.Aliases = AliasElement.LoadFrom(element).ToList();
            target.Classes = ClassElement.LoadFrom(element).ToList();
            target.Interfaces = InterfaceElement.LoadFrom(element).ToList();
            target.Records = RecordElement.LoadFrom(element).ToList();
            target.Enums = EnumerationElement.LoadFrom(element).ToList();
            target.Functions = FunctionElement.LoadFrom(element).ToList();
            target.FunctionMacros = FunctionMacroElement.LoadFrom(element).ToList();
            target.Unions = UnionElement.LoadFrom(element).ToList();
            target.BitFields = BitFieldElement.LoadFrom(element).ToList();
            target.Callbacks = CallbackElement.LoadFrom(element).ToList();
            target.Constants = ConstantElement.LoadFrom(element).ToList();
            target.Annotations = AnnotationElement.LoadFrom(element).ToList();
            target.Boxed = GObject.Introspection.Library.Model.BoxedElement.LoadFrom(element).ToList();
            return target;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="repository"></param>
        public NamespaceElement(RepositoryElement repository)
        {
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// Gets the <see cref="Repository"/> that holds this namespace.
        /// </summary>
        public RepositoryElement Repository { get; }

        /// <summary>
        /// Gets the name of the namespace.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the version of the namespace.
        /// </summary>
        public string Version { get; set; }

        public List<string> CIdentifierPrefixes { get; set; }

        public List<string> CSymbolPrefixes { get; set; }

        public string CPrefix { get; set; }

        public List<string> SharedLibraries { get; set; }

        /// <summary>
        /// Name of the library to specify for generated <see cref="DllImportAttribute"/> attributes.
        /// </summary>
        public string ClrSharedLibrary { get; set; }

        public List<PrimitiveElement> Primitives { get; set; }

        public List<AliasElement> Aliases { get; set; }

        public List<ClassElement> Classes { get; set; }

        public List<InterfaceElement> Interfaces { get; set; }

        public List<RecordElement> Records { get; set; }

        public List<EnumerationElement> Enums { get; set; }

        public List<FunctionElement> Functions { get; set; }

        public List<FunctionMacroElement> FunctionMacros { get; set; }

        public List<UnionElement> Unions { get; set; }

        public List<BitFieldElement> BitFields { get; set; }

        public List<CallbackElement> Callbacks { get; set; }

        public List<ConstantElement> Constants { get; set; }

        public List<AnnotationElement> Annotations { get; set; }

        public List<BoxedElement> Boxed { get; set; }

        public override string ToString()
        {
            return Name;
        }

    }

}
