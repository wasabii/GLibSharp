using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Gir.Xml
{

    public class Constructor : CallableWithSignature
    {

        public static new IEnumerable<Constructor> LoadFrom(XContainer container)
        {
            return CallableWithSignature.LoadFrom(container).OfType<Constructor>();
        }

        public static new Constructor Load(XElement element)
        {
            if (element.Name == Xmlns.Core_1_0_NS + "constructor")
                return Populate(new Constructor(), element);

            return null;
        }

        public static Constructor Populate(Constructor target, XElement element)
        {
            CallableWithSignature.Populate(target, element);
            return target;
        }

    }

}
