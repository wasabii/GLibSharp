using System.Xml.Linq;

namespace Gir.Xml
{

    /// <summary>
    /// Some base information for most elements like version, deprecation, stability, if they are introspectable or not, etc...
    /// </summary>
    public class Info
    {

        public static Info Load(XElement element)
        {
            return new Info()
            {
                Introspectable = element.Attribute("introspectable").ToBool(),
                Deprecated = element.Attribute("deprecated").ToBool(),
                DeprecatedVersion = (string)element.Attribute("deprecated-version"),
                Version = (string)element.Attribute("version"),
                Stability = element.Attribute("stability").ToEnum<Stability>(),
            };
        }

        /// <summary>
        /// Binary attribute which is false if the element is not introspectable.
        /// </summary>
        public bool? Introspectable { get; set; }

        /// <summary>
        /// Binary attribute which is true if the element has been deprecated.
        /// </summary>
        public bool? Deprecated { get; set; }

        /// <summary>
        /// Version number from which this element is deprecated.
        /// </summary>
        public string DeprecatedVersion { get; set; }

        /// <summary>
        /// Version number of an element.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Give the statibility status of the element.
        /// </summary>
        public Stability? Stability { get; set; }

    }

}
