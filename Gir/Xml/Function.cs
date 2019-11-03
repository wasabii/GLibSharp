using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Gir.Xml
{

    public class Function : CallableWithSignature
    {

        public static new IEnumerable<Function> LoadFrom(XContainer container)
        {
            return CallableWithSignature.LoadFrom(container).OfType<Function>();
        }

        public static new Function Load(XElement element)
        {
            if (element.Name == Xmlns.Core_1_0_NS + "function")
                return Populate(new Function(), element);

            return null;
        }

        public static Function Populate(Function target, XElement element)
        {
            CallableWithSignature.Populate(target, element);
            return target;
        }

    }

}
