using System.Xml.Serialization;

namespace GObject.Introspection.Model
{

    public enum Stability
    {

        [XmlEnum(null)]
        Unknown,

        [XmlEnum("stable")]
        Stable,

        [XmlEnum("unstable")]
        Unstable,

        [XmlEnum("private")]
        Private

    }

}
