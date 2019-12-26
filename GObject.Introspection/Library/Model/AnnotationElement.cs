using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Library.Model
{

    /// <summary>
    /// Element defining an annotation from the source code, usually a user-defined annotation associated to a parameter or a return value.
    /// </summary>
    public class AnnotationElement : Element
    {

        public static IEnumerable<AnnotationElement> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<AnnotationElement>();
        }

        public static AnnotationElement Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "attribute" ? Populate(new AnnotationElement(), element) : null;
        }

        public static AnnotationElement Populate(AnnotationElement target, XElement element)
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
