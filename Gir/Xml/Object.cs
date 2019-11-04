using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Gir.Xml
{

    public class Object : Structure
    {

        public static new IEnumerable<Object> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<Object>();
        }

        public static new Object Load(XElement element)
        {
            return (Object)Class.Load(element) ?? (Object)Record.Load(element);
        }

        public static Object Populate(Object target, XElement element)
        {
            Structure.Populate(target, element);
            target.Properties = Property.LoadFrom(element).ToList();
            return target;
        }

        public List<Property> Properties { get; set; }

    }

}
