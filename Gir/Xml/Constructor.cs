using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Gir.Xml
{

    public class Constructor : CallableWithSignature
    {

        public static new IEnumerable<Constructor> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<Constructor>();
        }

        public static new Constructor Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "constructor" ? Populate(new Constructor(), element) : null;
        }

        public static Constructor Populate(Constructor target, XElement element)
        {
            CallableWithSignature.Populate(target, element);
            return target;
        }

    }

}
