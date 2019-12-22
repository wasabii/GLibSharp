using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Xml
{

    public class RecordElement : ObjectElement
    {

        public static new IEnumerable<RecordElement> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<RecordElement>();
        }

        public static new RecordElement Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "record" ? Populate(new RecordElement(), element) : null;
        }

        public static RecordElement Populate(RecordElement target, XElement element)
        {
            ObjectElement.Populate(target, element);
            target.Disguised = element.Attribute("disguised").ToBool();
            target.Foreign = element.Attribute("foreign").ToBool();
            target.GLibIsGTypeStructFor = (string)element.Attribute(Xmlns.GLib_1_0_NS + "is-gtype-struct-for");
            target.Unions = UnionElement.LoadFrom(element).ToList();
            return target;
        }

        public bool? Disguised { get; set; }

        public bool? Foreign { get; set; }

        public string GLibIsGTypeStructFor { get; set; }

        public List<UnionElement> Unions { get; set; }

        public override string ToString()
        {
            return Name ?? GLibTypeName ?? CType;
        }

    }

}
