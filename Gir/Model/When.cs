using System.Xml.Serialization;

namespace Gir.Model
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
