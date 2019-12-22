using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Xml
{

    public class PackageElement : Element
    {

        public static IEnumerable<PackageElement> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<PackageElement>();
        }

        public static PackageElement Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "package" ? Populate(new PackageElement(), element) : null;
        }

        public static PackageElement Populate(PackageElement target, XElement element)
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
