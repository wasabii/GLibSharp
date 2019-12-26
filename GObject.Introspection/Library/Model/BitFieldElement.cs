using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Library.Model
{

    /// <summary>
    /// Element defining a bit field (as in C).
    /// </summary>
    public class BitFieldElement : FlagElement
    {

        public static new IEnumerable<BitFieldElement> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<BitFieldElement>();
        }

        public static new BitFieldElement Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "bitfield" ? Populate(new BitFieldElement(), element) : null;
        }

        public static BitFieldElement Populate(BitFieldElement target, XElement element)
        {
            FlagElement.Populate(target, element);
            return target;
        }

    }

}
