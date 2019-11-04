using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Gir.Xml
{

    public class Record : Object, IHasInfo
    {

        public static new IEnumerable<Record> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<Record>();
        }

        public static new Record Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "record" ? Populate(new Record(), element) : null;
        }

        public static Record Populate(Record target, XElement element)
        {
            Object.Populate(target, element);
            target.Disguised = element.Attribute("disguised").ToBool();
            target.Foreign = element.Attribute("foreign").ToBool();
            target.GLibIsGTypeStructFor = (string)element.Attribute(Xmlns.GLib_1_0_NS + "is-gtype-struct-for");
            target.Unions = Union.LoadFrom(element).ToList();
            return target;
        }

        public bool? Disguised { get; set; }

        public bool? Foreign { get; set; }

        public string GLibIsGTypeStructFor { get; set; }

        public List<Union> Unions { get; set; }

        public override string ToString()
        {
            return Name ?? GLibTypeName ?? CType;
        }

    }

}
