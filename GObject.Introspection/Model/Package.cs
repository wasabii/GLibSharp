using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Model
{

    public class Package : Element
    {

        public static IEnumerable<Package> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<Package>();
        }

        public static Package Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "package" ? Populate(new Package(), element) : null;
        }

        public static Package Populate(Package target, XElement element)
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
