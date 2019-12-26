using System.Xml.Serialization;

namespace GObject.Introspection.Library.Model
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