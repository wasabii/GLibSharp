using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Gir.Xml
{

    public class CInclude
    {

        public static IEnumerable<CInclude> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<CInclude>();
        }

        public static CInclude Load(XElement element)
        {
            if (element.Name == Xmlns.C_1_0_NS + "include")
                return Populate(new CInclude(), element);

            return null;
        }

        public static CInclude Populate(CInclude target, XElement element)
        {
            target.Name = (string)element.Attribute("name");
            return target;
        }

        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }

    }

}
