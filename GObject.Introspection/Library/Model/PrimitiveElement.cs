using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Library.Model
{

    public class PrimitiveElement : Element, IHasName, IHasCType, IHasInfo, IHasClrInfo
    {

        public static IEnumerable<PrimitiveElement> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<PrimitiveElement>();
        }

        public static PrimitiveElement Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "primitive" ? Populate(new PrimitiveElement(), element) : null;
        }

        public static PrimitiveElement Populate(PrimitiveElement target, XElement element)
        {
            Element.Populate(target, element);
            target.Info = Info.Load(element);
            target.Documentation = Documentation.Load(element);
            target.Annotations = AnnotationElement.LoadFrom(element).ToList();
            target.Name = (string)element.Attribute("name");
            target.CType = (string)element.Attribute(Xmlns.C_1_0_NS + "type");
            target.GLibTypeName = (string)element.Attribute(Xmlns.GLib_1_0_NS + "type-name");
            target.ClrInfo = ClrInfo.Load(element);
            return target;
        }

        public Info Info { get; set; }

        public Documentation Documentation { get; set; }

        public List<AnnotationElement> Annotations { get; set; }

        public string Name { get; set; }

        public string CType { get; set; }

        public string GLibTypeName { get; set; }

        public ClrInfo ClrInfo { get; set; }

        public override string ToString()
        {
            return Name ?? GLibTypeName ?? CType;
        }

    }

}
