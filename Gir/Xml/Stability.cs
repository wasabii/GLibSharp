using System.Xml.Serialization;

namespace Gir.Xml
{

    public enum Stability
    {

        [XmlEnum("stable")]
        Stable,

        [XmlEnum("unstable")]
        Unstable,

        [XmlEnum("private")]
        Private

    }

}
