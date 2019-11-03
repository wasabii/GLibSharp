using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Gir.Xml
{

    public class Package
    {

        public static IEnumerable<Package> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<Package>();
        }

        public static Package Load(XElement element)
        {
            if (element.Name == Xmlns.Core_1_0_NS + "package")
                return Populate(new Package(), element);

            return null;
        }

        public static Package Populate(Package target, XElement element)
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
