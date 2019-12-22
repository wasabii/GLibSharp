using System.Xml.Serialization;

namespace GObject.Introspection.Xml
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