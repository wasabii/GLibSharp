using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Model
{

    /// <summary>
    /// Documentation of elements.
    /// </summary>
    public class Documentation
    {

        public static Documentation Load(XElement element)
        {
            return new Documentation()
            {
                Version = (string)element.Element(Xmlns.Core_1_0_NS + "doc-version"),
                Stability = (string)element.Element(Xmlns.Core_1_0_NS + "doc-stability"),
                Deprecated = (string)element.Element(Xmlns.Core_1_0_NS + "doc-deprecated"),
                Filename = (string)element.Element(Xmlns.Core_1_0_NS + "doc")?.Attribute("filename"),
                Line = (string)element.Element(Xmlns.Core_1_0_NS + "doc")?.Attribute("line"),
                Column = (string)element.Element(Xmlns.Core_1_0_NS + "doc")?.Attribute("column"),
                Text = (string)element.Element(Xmlns.Core_1_0_NS + "doc"),
                SourcePosition = element.Elements(Xmlns.Core_1_0_NS + "source-position").Select(i => SourcePosition.Load(i)).OfType<SourcePosition>().FirstOrDefault(),
            };
        }

        public string Version { get; set; }

        /// <summary>
        /// A text value about the stability of the documentation. Usually a simple description like stable or unstable.
        /// </summary>
        public string Stability { get; set; }

        /// <summary>
        /// Deprecated documentation of an element. Kept for historical reasons in general.
        /// </summary>
        public string Deprecated { get; set; }

        /// <summary>
        /// The file containing this documentation.
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// The first line of the documentation in the source code.
        /// </summary>
        public string Line { get; set; }

        /// <summary>
        /// The first column of the documentation in the source code.
        /// </summary>
        public string Column { get; set; }

        /// <summary>
        /// The text of the documentation.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Position of the documentation in the original source code.
        /// </summary>
        public SourcePosition SourcePosition { get; set; }
    }

}
