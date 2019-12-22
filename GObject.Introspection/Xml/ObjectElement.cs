using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Xml
{

    public class ObjectElement : StructureElement
    {

        public static new IEnumerable<ObjectElement> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<ObjectElement>();
        }

        public static new ObjectElement Load(XElement element)
        {
            return (ObjectElement)ClassElement.Load(element) ?? (ObjectElement)RecordElement.Load(element);
        }

        public static ObjectElement Populate(ObjectElement target, XElement element)
        {
            StructureElement.Populate(target, element);
            target.Properties = PropertyElement.LoadFrom(element).ToList();
            return target;
        }

        public List<PropertyElement> Properties { get; set; }

    }

}
