using System.Xml.Serialization;

namespace GObject.Introspection.Xml
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
