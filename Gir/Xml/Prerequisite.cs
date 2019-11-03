using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Gir.Xml
{

    /// <summary>
    /// Interface which is pre-required to implement another interface. This node is generally using within an interface element.
    /// </summary>
    public class Prerequisite
    {

        public static IEnumerable<Prerequisite> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<Prerequisite>();
        }

        public static Prerequisite Load(XElement element)
        {
            if (element.Name == Xmlns.Core_1_0_NS + "prerequisite")
                return Populate(new Prerequisite(), element);

            return null;
        }

        public static Prerequisite Populate(Prerequisite target, XElement element)
        {
            target.Name = (string)element.Attribute("name");
            return target;
        }

        /// <summary>
        /// Name of the required interface.
        /// </summary>
        public string Name { get; set; }

    }

}
