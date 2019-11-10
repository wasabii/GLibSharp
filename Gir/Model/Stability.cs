using System.Xml.Serialization;

namespace Gir.Model
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
