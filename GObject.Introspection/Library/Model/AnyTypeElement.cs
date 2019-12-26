using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Library.Model
{

    public abstract class AnyTypeElement : Element
    {

        public static IEnumerable<AnyTypeElement> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<AnyTypeElement>();
        }

        public static AnyTypeElement Load(XElement element)
        {
            return (AnyTypeElement)TypeElement.Load(element) ?? (AnyTypeElement)ArrayTypeElement.Load(element);
        }

        public static AnyTypeElement Populate(AnyTypeElement target, XElement element)
        {
            Element.Populate(target, element);
            target.Name = (string)element.Attribute("name");
            target.CType = (string)element.Attribute(Xmlns.C_1_0_NS + "type");
            target.Introspectable = element.Attribute("introspectable").ToBool();
            return target;
        }

        /// <summary>
        /// Name of the type.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The C representation of hte type.
        /// </summary>
        public string CType { get; set; }

        /// <summary>
        /// Binary attribute which is false if the element is not introspectable.
        /// </summary>
        public bool? Introspectable { get; set; }

    }

}
