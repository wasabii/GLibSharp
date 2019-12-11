using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Model
{

    public class Primitive : Element, IHasName, IHasInfo, IHasClrInfo
    {

        public static IEnumerable<Primitive> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<Primitive>();
        }

        public static Primitive Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "primitive" ? Populate(new Primitive(), element) : null;
        }

        public static Primitive Populate(Primitive target, XElement element)
        {
            Element.Populate(target, element);
            target.Info = Info.Load(element);
            target.Documentation = Documentation.Load(element);
            target.Annotations = Annotation.LoadFrom(element).ToList();
            target.Name = (string)element.Attribute("name");
            target.CType = (string)element.Attribute(Xmlns.C_1_0_NS + "type");
            target.GLibTypeName = (string)element.Attribute(Xmlns.GLib_1_0_NS + "type-name");
            target.ClrInfo = ClrInfo.Load(element);
            return target;
        }

        public Info Info { get; set; }

        public Documentation Documentation { get; set; }

        public List<Annotation> Annotations { get; set; }

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
