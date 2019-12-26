using System.Xml.Serialization;

namespace GObject.Introspection.Library.Model
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