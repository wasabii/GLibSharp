using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Library.Model
{

    public class MethodElement : CallableWithSignatureElement
    {

        public static new IEnumerable<MethodElement> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<MethodElement>();
        }

        public static new MethodElement Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "method" ? Populate(new MethodElement(), element) : null;
        }

        public static MethodElement Populate(MethodElement target, XElement element)
        {
            CallableWithSignatureElement.Populate(target, element);
            return target;
        }

    }

}
