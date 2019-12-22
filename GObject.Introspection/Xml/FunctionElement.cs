using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Xml
{

    public class FunctionElement : CallableWithSignatureElement
    {

        public static new IEnumerable<FunctionElement> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<FunctionElement>();
        }

        public static new FunctionElement Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "function" ? Populate(new FunctionElement(), element) : null;
        }

        public static FunctionElement Populate(FunctionElement target, XElement element)
        {
            CallableWithSignatureElement.Populate(target, element);
            return target;
        }

    }

}
