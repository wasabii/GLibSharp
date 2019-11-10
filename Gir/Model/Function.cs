using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Gir.Model
{

    public class Function : CallableWithSignature
    {

        public static new IEnumerable<Function> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<Function>();
        }

        public static new Function Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "function" ? Populate(new Function(), element) : null;
        }

        public static Function Populate(Function target, XElement element)
        {
            CallableWithSignature.Populate(target, element);
            return target;
        }

    }

}
