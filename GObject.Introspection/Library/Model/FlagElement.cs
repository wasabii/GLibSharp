using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Library.Model
{

    /// <summary>
    /// Element defining a bit field (as in C).
    /// </summary>
    public abstract class FlagElement : Element, IHasName, IHasInfo, IHasClrInfo
    {

        public static IEnumerable<FlagElement> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<FlagElement>();
        }

        public static FlagElement Load(XElement element)
        {
            return (FlagElement)EnumerationElement.Load(element) ?? (FlagElement)BitFieldElement.Load(element);
        }

        public static FlagElement Populate(FlagElement target, XElement element)
        {
            Element.Populate(target, element);
            target.Info = Info.Load(element);
            target.Documentation = Documentation.Load(element);
            target.Annotations = AnnotationElement.LoadFrom(element).ToList();
            target.Name = (string)element.Attribute("name");
            target.CType = (string)element.Attribute(Xmlns.C_1_0_NS + "type");
            target.GLibTypeName = (string)element.Attribute(Xmlns.GLib_1_0_NS + "type-name");
            target.GLibGetType = (string)element.Attribute(Xmlns.GLib_1_0_NS + "get-type");
            target.Members = MemberElement.LoadFrom(element).ToList();
            target.Functions = FunctionElement.LoadFrom(element).ToList();
            target.ClrInfo = ClrInfo.Load(element);
            return target;
        }

        public Info Info { get; set; }

        public Documentation Documentation { get; set; }

        public List<AnnotationElement> Annotations { get; set; }

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

        public List<MemberElement> Members { get; set; }

        public List<FunctionElement> Functions { get; set; }

        public ClrInfo ClrInfo { get; set; }

        public override string ToString()
        {
            return Name ?? GLibTypeName ?? CType;
        }

    }

}
