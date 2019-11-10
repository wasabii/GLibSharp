using System.Xml.Serialization;

namespace Gir.Model
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