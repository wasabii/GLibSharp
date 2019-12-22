using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Xml
{

    public class VirtualMethodElement : CallableWithSignatureElement
    {

        public static new IEnumerable<VirtualMethodElement> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<VirtualMethodElement>();
        }

        public static new VirtualMethodElement Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "virtual-method" ? Populate(new VirtualMethodElement(), element) : null;
        }

        public static VirtualMethodElement Populate(VirtualMethodElement target, XElement element)
        {
            CallableWithSignatureElement.Populate(target, element);
            target.Invoker = (string)element.Attribute("invoker");
            return target;
        }

        public string Invoker { get; set; }

    }

}
