using System.Xml.Serialization;

namespace Gir.Model
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