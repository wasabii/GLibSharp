using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Library.Model
{

    public class UnionElement : StructureElement
    {

        public static new IEnumerable<UnionElement> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<UnionElement>();
        }

        public static new UnionElement Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "union" ? Populate(new UnionElement(), element) : null;
        }

        public static UnionElement Populate(UnionElement target, XElement element)
        {
            StructureElement.Populate(target, element);
            target.Records = RecordElement.LoadFrom(element).ToList();
            return target;
        }

        public List<RecordElement> Records { get; set; }

        public override string ToString()
        {
            return Name ?? GLibTypeName ?? CType;
        }

    }

}
