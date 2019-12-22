using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Xml
{

    public class IncludeElement : Element
    {

        public static IEnumerable<IncludeElement> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<IncludeElement>();
        }

        public static IncludeElement Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "include" ? Populate(new IncludeElement(), element) : null;
        }

        public static IncludeElement Populate(IncludeElement target, XElement element)
        {
            Element.Populate(target, element);
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
