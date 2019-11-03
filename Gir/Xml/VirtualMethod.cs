using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Gir.Xml
{

    public class VirtualMethod : CallableWithSignature
    {

        public static new IEnumerable<VirtualMethod> LoadFrom(XContainer container)
        {
            return CallableWithSignature.LoadFrom(container).OfType<VirtualMethod>();
        }

        public static new VirtualMethod Load(XElement element)
        {
            if (element.Name == Xmlns.Core_1_0_NS + "virtual-method")
                return Populate(new VirtualMethod(), element);

            return null;
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
