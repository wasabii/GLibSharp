using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Gir.Xml
{

    public class Include
    {

        public static IEnumerable<Include> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<Include>();
        }

        public static Include Load(XElement element)
        {
            if (element.Name == Xmlns.Core_1_0_NS + "include")
                return Populate(new Include(), element);

            return null;
        }

        public static Include Populate(Include target, XElement element)
        {
            target.Name = (string)element.Attribute("name");
            target.Version = (string)element.Attribute("version");
            return target;
        }

        public string Name { get; set; }

        public string Version { get; set; }

        public override string ToString()
        {
            return Name;
        }

    }

}
