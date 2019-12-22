using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace GObject.Introspection.Model
{

    public class Namespace : Element
    {

        public static IEnumerable<Namespace> LoadFrom(Repository repository, XContainer container)
        {
            return container.Elements().Select(i => Load(repository, i)).OfType<Namespace>();
        }

        public static Namespace Load(Repository repository, XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "namespace" ? Populate(new Namespace(repository), element) : null;
        }

        public static Namespace Populate(Namespace target, XElement element)
        {
            Element.Populate(target, element);
            target.Name = (string)element.Attribute("name");
            target.Version = (string)element.Attribute("version");
            target.CIdentifierPrefixes = XmlUtil.ParseStringList((string)element.Attribute(Xmlns.C_1_0_NS + "identifier-prefixes"));
            target.CSymbolPrefixes = XmlUtil.ParseStringList((string)element.Attribute(Xmlns.C_1_0_NS + "symbol-prefixes"));
            target.CPrefix = (string)element.Attribute(Xmlns.C_1_0_NS + "prefix");
            target.SharedLibraries = XmlUtil.ParseStringList((string)element.Attribute("shared-library"));
            target.ClrSharedLibrary = (string)element.Attribute(Xmlns.CLR_1_0_NS + "shared-library");
            target.Primitives = Primitive.LoadFrom(element).ToList();
            target.Aliases = Alias.LoadFrom(element).ToList();
            target.Classes = Class.LoadFrom(element).ToList();
            target.Interfaces = Interface.LoadFrom(element).ToList();
            target.Records = Record.LoadFrom(element).ToList();
            target.Enums = Enumeration.LoadFrom(element).ToList();
            target.Functions = Function.LoadFrom(element).ToList();
            target.FunctionMacros = FunctionMacro.LoadFrom(element).ToList();
            target.Unions = Union.LoadFrom(element).ToList();
            target.BitFields = BitField.LoadFrom(element).ToList();
            target.Callbacks = Callback.LoadFrom(element).ToList();
            target.Constants = Constant.LoadFrom(element).ToList();
            target.Annotations = Annotation.LoadFrom(element).ToList();
            target.Boxed = GObject.Introspection.Model.Boxed.LoadFrom(element).ToList();
            return target;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="repository"></param>
        public Namespace(Repository repository)
        {
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// Gets the <see cref="Repository"/> that holds this namespace.
        /// </summary>
        public Repository Repository { get; }

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

        public List<Primitive> Primitives { get; set; }

        public List<Alias> Aliases { get; set; }

        public List<Class> Classes { get; set; }

        public List<Interface> Interfaces { get; set; }

        public List<Record> Records { get; set; }

        public List<Enumeration> Enums { get; set; }

        public List<Function> Functions { get; set; }

        public List<FunctionMacro> FunctionMacros { get; set; }

        public List<Union> Unions { get; set; }

        public List<BitField> BitFields { get; set; }

        public List<Callback> Callbacks { get; set; }

        public List<Constant> Constants { get; set; }

        public List<Annotation> Annotations { get; set; }

        public List<Boxed> Boxed { get; set; }

        public override string ToString()
        {
            return Name;
        }

    }

}
