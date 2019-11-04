using System.Xml.Serialization;

namespace Gir.Xml
{

    public enum ParameterDirection
    {

        [XmlEnum("out")]
        Out,

        [XmlEnum("in")]
        In,

        [XmlEnum("inout")]
        InOut,

    }

}