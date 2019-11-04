using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Gir.Xml
{

    public class Method : CallableWithSignature
    {

        public static new IEnumerable<Method> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<Method>();
        }

        public static new Method Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "method" ? Populate(new Method(), element) : null;
        }

        public static Method Populate(Method target, XElement element)
        {
            CallableWithSignature.Populate(target, element);
            return target;
        }

    }

}
