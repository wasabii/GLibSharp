using System.Xml.Linq;

namespace Gir.Model
{

    public abstract class Element
    {

        public static Element Populate(Element target, XElement element)
        {
            return target;
        }

    }

}
