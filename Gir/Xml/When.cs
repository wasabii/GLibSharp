using System.Xml.Serialization;

namespace Gir.Xml
{

    public enum When
    {

        [XmlEnum("first")]
        First,

        [XmlEnum("last")]
        Last,

        [XmlEnum("cleanup")]
        Cleanup

    }

}
