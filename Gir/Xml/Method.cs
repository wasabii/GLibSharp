using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Gir.Xml
{

    public class Method : CallableWithSignature
    {

        public static new IEnumerable<Method> LoadFrom(XContainer container)
        {
            return CallableWithSignature.LoadFrom(container).OfType<Method>();
        }

        public static new Method Load(XElement element)
        {
            if (element.Name == Xmlns.Core_1_0_NS + "method")
                return Populate(new Method(), element);

            return null;
        }

        public static Method Populate(Method target, XElement element)
        {
            CallableWithSignature.Populate(target, element);
            return target;
        }

    }

}
