using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Library.Model
{

    /// <summary>
    /// Give the name of the interface it implements. This element is generally used within a class element.
    /// </summary>
    public class ImplementsElement : Element
    {

        public static IEnumerable<ImplementsElement> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<ImplementsElement>();
        }

        public static ImplementsElement Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "implements" ? Populate(new ImplementsElement(), element) : null;
        }

        public static ImplementsElement Populate(ImplementsElement target, XElement element)
        {
            Element.Populate(target, element);
            target.Name = (string)element.Attribute("name");
            return target;
        }

        /// <summary>
        /// Name of the interface implemented by a class.
        /// </summary>
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }

    }

}
