using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Model
{

    public class VirtualMethod : CallableWithSignature
    {

        public static new IEnumerable<VirtualMethod> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<VirtualMethod>();
        }

        public static new VirtualMethod Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "virtual-method" ? Populate(new VirtualMethod(), element) : null;
        }

        public static VirtualMethod Populate(VirtualMethod target, XElement element)
        {
            CallableWithSignature.Populate(target, element);
            target.Invoker = (string)element.Attribute("invoker");
            return target;
        }

        public string Invoker { get; set; }

    }

}
