using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Gir.Xml
{

    public class Union : Structure
    {

        public static new IEnumerable<Union> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<Union>();
        }

        public static new Union Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "union" ? Populate(new Union(), element) : null;
        }

        public static Union Populate(Union target, XElement element)
        {
            Structure.Populate(target, element);
            target.Records = Record.LoadFrom(element).ToList();
            return target;
        }

        public List<Record> Records { get; set; }

        public override string ToString()
        {
            return Name ?? GLibTypeName ?? CType;
        }

    }

}
