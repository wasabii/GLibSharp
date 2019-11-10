using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Gir.Model
{

    /// <summary>
    /// Give the name of the interface it implements. This element is generally used within a class element.
    /// </summary>
    public class Implements : Element
    {

        public static IEnumerable<Implements> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<Implements>();
        }

        public static Implements Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "implements" ? Populate(new Implements(), element) : null;
        }

        public static Implements Populate(Implements target, XElement element)
        {
            Element.Populate(target, element);
            target.Name = (string)element.Attribute("name");
            return target;
        }

        /// <summary>
        /// Name of the interface implemented by a class.
        /// </summary>
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }

    }

}
