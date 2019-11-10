using System.Xml.Linq;

namespace Gir.Model
{

    public class ClrInfo
    {

        public static ClrInfo Load(XElement element)
        {
            return new ClrInfo()
            {
                Name = (string)element.Attribute(Xmlns.CLR_1_0_NS + "name"),
                Marshaler = (string)element.Attribute(Xmlns.CLR_1_0_NS + "marshaler")
            };
        }

        public string Name { get; set; }

        public string Marshaler { get; set; }

    }

}
