using System;
using System.Xml.Linq;

namespace GObject.Introspection.Xml
{

    public class ClrInfo
    {

        public static ClrInfo Load(XElement element)
        {
            return new ClrInfo()
            {
                Type = (string)element.Attribute(Xmlns.CLR_1_0_NS + "type"),
                MarshalerType = (string)element.Attribute(Xmlns.CLR_1_0_NS + "marshaler-type"),
                Kind = (string)element.Attribute(Xmlns.CLR_1_0_NS + "kind") is string s ? (ClrObjectKind)Enum.Parse(typeof(ClrObjectKind), s) : ClrObjectKind.Auto,
            };
        }

        public string Type { get; set; }

        public string NullableType { get; set; }

        public string NullExpression { get; set; }

        public string MarshalerType { get; set; }

        public ClrObjectKind Kind { get; set; }

    }

}
