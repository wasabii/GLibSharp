using System.Collections.Generic;
using System.Linq;
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
                Introspectable = (int?)element.Attribute("introspectable") != 0,
                Deprecated = (int?)element.Attribute("introspectable") == 1,
                DeprecatedVersion = (string)element.Attribute("deprecated-version"),
                Version = (string)element.Attribute("version"),
                Stability = XmlUtil.ParseEnum<Stability>((string)element.Attribute("stability")),
            };
        }

        /// <summary>
        /// Binary attribute which is false if the element is not introspectable.
        /// </summary>
        public bool Introspectable { get; set; }

        /// <summary>
        /// Binary attribute which is true if the element has been deprecated.
        /// </summary>
        public bool Deprecated { get; set; }

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
