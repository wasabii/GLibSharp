using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace Gir.Model
{

    public class Namespace : Element
    {

        public static IEnumerable<Namespace> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<Namespace>();
        }

        public static Namespace Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "namespace" ? Populate(new Namespace(), element) : null;
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
            target.Unions = Union.LoadFrom(element).ToList();
            target.BitFields = BitField.LoadFrom(element).ToList();
            target.Callbacks = Callback.LoadFrom(element).ToList();
            target.Constants = Constant.LoadFrom(element).ToList();
            target.Annotations = Annotation.LoadFrom(element).ToList();
            target.Boxed = Gir.Model.Boxed.LoadFrom(element).ToList();
            return target;
        }

        public string Name { get; set; }

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
