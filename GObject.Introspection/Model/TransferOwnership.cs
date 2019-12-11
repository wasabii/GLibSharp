using System.Xml.Serialization;

namespace GObject.Introspection.Model
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