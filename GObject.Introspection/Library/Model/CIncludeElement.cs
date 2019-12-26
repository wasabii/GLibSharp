using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Library.Model
{

    public class CIncludeElement : Element
    {

        public static IEnumerable<CIncludeElement> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<CIncludeElement>();
        }

        public static CIncludeElement Load(XElement element)
        {
            return element.Name == Xmlns.C_1_0_NS + "include" ? Populate(new CIncludeElement(), element) : null;
        }

        public static CIncludeElement Populate(CIncludeElement target, XElement element)
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
