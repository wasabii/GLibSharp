using System.Xml.Linq;

namespace Gir.Model
{

    public class ClrInfo
    {

        public static ClrInfo Load(XElement element)
        {
            return new ClrInfo()
            {
                Type = (string)element.Attribute(Xmlns.CLR_1_0_NS + "type"),
                MarshalerType = (string)element.Attribute(Xmlns.CLR_1_0_NS + "marshaler-type")
            };
        }

        public string Type { get; set; }

        public string NullableType { get; set; }

        public string NullExpression { get; set; }

        public string MarshalerType { get; set; }

    }

}
