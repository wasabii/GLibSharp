using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Gir.Xml
{

    /// <summary>
    /// Element defining a bit field (as in C).
    /// </summary>
    public class BitField : Element, IHasName, IHasInfo
    {

        public static IEnumerable<BitField> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<BitField>();
        }

        public static BitField Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "bitfield" ? Populate(new BitField(), element) : null;
        }

        public static BitField Populate(BitField target, XElement element)
        {
            Element.Populate(target, element);
            target.Info = Info.Load(element);
            target.Documentation = Documentation.Load(element);
            target.Annotations = Annotation.LoadFrom(element).ToList();
            target.Name = (string)element.Attribute("name");
            target.CType = (string)element.Attribute(Xmlns.C_1_0_NS + "type");
            target.GLibTypeName = (string)element.Attribute(Xmlns.GLib_1_0_NS + "type-name");
            target.GLibGetType = (string)element.Attribute(Xmlns.GLib_1_0_NS + "get-type");
            target.Members = Member.LoadFrom(element).ToList();
            target.Functions = Function.LoadFrom(element).ToList();
            return target;
        }

        public Info Info { get; set; }

        public Documentation Documentation { get; set; }

        public List<Annotation> Annotations { get; set; }

        /// <summary>
        /// Name of the bit field.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Corresponding C type of the bit field type.
        /// </summary>
        public string CType { get; set; }

        /// <summary>
        /// GObject compatible type name.
        /// </summary>
        public string GLibTypeName { get; set; }

        /// <summary>
        /// Function to retrieve the GObject compatible type of the element.
        /// </summary>
        public string GLibGetType { get; set; }

        public List<Member> Members { get; set; }

        public List<Function> Functions { get; set; }

        public override string ToString()
        {
            return Name ?? GLibTypeName ?? CType;
        }

    }

}
