using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Gir.Model
{

    /// <summary>
    /// Interface which is pre-required to implement another interface. This node is generally using within an interface element.
    /// </summary>
    public class Prerequisite : Element
    {

        public static IEnumerable<Prerequisite> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<Prerequisite>();
        }

        public static Prerequisite Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "prerequisite" ? Populate(new Prerequisite(), element) : null;
        }

        public static Prerequisite Populate(Prerequisite target, XElement element)
        {
            Element.Populate(target, element);
            target.Name = (string)element.Attribute("name");
            return target;
        }

        /// <summary>
        /// Name of the required interface.
        /// </summary>
        public string Name { get; set; }

    }

}
