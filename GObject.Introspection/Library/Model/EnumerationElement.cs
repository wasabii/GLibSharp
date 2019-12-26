using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Library.Model
{

    public class EnumerationElement : FlagElement
    {

        public static new IEnumerable<EnumerationElement> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<EnumerationElement>();
        }

        public static new EnumerationElement Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "enumeration" ? Populate(new EnumerationElement(), element) : null;
        }

        public static EnumerationElement Populate(EnumerationElement target, XElement element)
        {
            FlagElement.Populate(target, element);
            target.GLibErrorDomain = (string)element.Attribute(Xmlns.GLib_1_0_NS + "error-domain");
            return target;
        }

        public string GLibErrorDomain { get; set; }

    }

}

