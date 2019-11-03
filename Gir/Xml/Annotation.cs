using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Gir.Xml
{

    /// <summary>
    /// Element defining an annotation from the source code, usually a user-defined annotation associated to a parameter or a return value.
    /// </summary>
    public class Annotation
    {

        public static IEnumerable<Annotation> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<Annotation>();
        }

        public static Annotation Load(XElement element)
        {
            if (element.Name == Xmlns.Core_1_0_NS + "attribute")
                return Populate(new Annotation(), element);

            return null;
        }

        public static Annotation Populate(Annotation target, XElement element)
        {
            target.Name = (string)element.Attribute("name");
            target.Value = (string)element.Attribute("value");
            return target;
        }

        public string Name { get; set; }

        public string Value { get; set; }

        public override string ToString()
        {
            return Name;
        }

    }

}
