using System.Xml.Serialization;

namespace GObject.Introspection.Xml
{

    public enum TransferOwnership
    {

        [XmlEnum("none")]
        None,

        [XmlEnum("container")]
        Container,

        [XmlEnum("full")]
        Full,

    }

}