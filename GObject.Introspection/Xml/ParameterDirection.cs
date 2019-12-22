using System.Xml.Serialization;

namespace GObject.Introspection.Xml
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