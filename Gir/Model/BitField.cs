using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Gir.Model
{

    /// <summary>
    /// Element defining a bit field (as in C).
    /// </summary>
    public class BitField : Flag
    {

        public static new IEnumerable<BitField> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<BitField>();
        }

        public static new BitField Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "bitfield" ? Populate(new BitField(), element) : null;
        }

        public static BitField Populate(BitField target, XElement element)
        {
            Flag.Populate(target, element);
            return target;
        }

    }

}
