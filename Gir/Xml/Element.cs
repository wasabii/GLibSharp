using System.Xml.Linq;

namespace Gir.Xml
{

    public abstract class Element
    {

        public static Element Populate(Element target, XElement element)
        {
            return target;
        }

    }

}
