using System.Xml.Serialization;

namespace Gir.Xml
{

    public enum ValueScope
    {

        [XmlEnum("notified")]
        Notified,

        [XmlEnum("async")]
        Async,

        [XmlEnum("call")]
        Call

    }

}