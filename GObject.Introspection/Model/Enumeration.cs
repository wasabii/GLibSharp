using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Model
{

    public class Enumeration : Flag
    {

        public static new IEnumerable<Enumeration> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<Enumeration>();
        }

        public static new Enumeration Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "enumeration" ? Populate(new Enumeration(), element) : null;
        }

        public static Enumeration Populate(Enumeration target, XElement element)
        {
            Flag.Populate(target, element);
            target.GLibErrorDomain = (string)element.Attribute(Xmlns.GLib_1_0_NS + "error-domain");
            return target;
        }

        public string GLibErrorDomain { get; set; }

    }

}

