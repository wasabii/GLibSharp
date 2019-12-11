using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Model
{

    /// <summary>
    /// Element defining an annotation from the source code, usually a user-defined annotation associated to a parameter or a return value.
    /// </summary>
    public class Annotation : Element
    {

        public static IEnumerable<Annotation> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<Annotation>();
        }

        public static Annotation Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "attribute" ? Populate(new Annotation(), element) : null;
        }

        public static Annotation Populate(Annotation target, XElement element)
        {
            Element.Populate(target, element);
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
