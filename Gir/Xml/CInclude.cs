using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Gir.Xml
{

    public class CInclude : Element
    {

        public static IEnumerable<CInclude> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<CInclude>();
        }

        public static CInclude Load(XElement element)
        {
            return element.Name == Xmlns.C_1_0_NS + "include" ? Populate(new CInclude(), element) : null;
        }

        public static CInclude Populate(CInclude target, XElement element)
        {
            Element.Populate(target, element);
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
