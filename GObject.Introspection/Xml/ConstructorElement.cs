using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Xml
{

    public class ConstructorElement : CallableWithSignatureElement
    {

        public static new IEnumerable<ConstructorElement> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<ConstructorElement>();
        }

        public static new ConstructorElement Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "constructor" ? Populate(new ConstructorElement(), element) : null;
        }

        public static ConstructorElement Populate(ConstructorElement target, XElement element)
        {
            CallableWithSignatureElement.Populate(target, element);
            return target;
        }

    }

}
