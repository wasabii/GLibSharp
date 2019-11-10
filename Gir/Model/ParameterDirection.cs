using System.Xml.Serialization;

namespace Gir.Model
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