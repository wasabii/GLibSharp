using System.Xml.Linq;

namespace Gir.Model
{

    /// <summary>
    /// Position of the documentation in the original source code.
    /// </summary>
    public class SourcePosition
    {

        public static SourcePosition Load(XElement element)
        {
            return Populate(new SourcePosition(), element);
        }

        public static SourcePosition Populate(SourcePosition target, XElement element)
        {
            target.Filename = (string)element.Attribute("filename");
            target.Line = (string)element.Attribute("line");
            target.Column = (string)element.Attribute("column");
            return target;
        }

        /// <summary>
        /// File name of the source of the documentation.
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

    }

}
