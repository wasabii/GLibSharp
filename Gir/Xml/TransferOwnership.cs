using System.Xml.Serialization;

namespace Gir.Xml
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