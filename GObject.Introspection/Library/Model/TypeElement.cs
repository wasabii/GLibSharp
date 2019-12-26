using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Library.Model
{

    /// <summary>
    /// A simple type of data (as opposed to an array).
    /// </summary>
    public class TypeElement : AnyTypeElement, IHasDocumentation
    {

        public static new IEnumerable<TypeElement> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<TypeElement>();
        }

        public static new TypeElement Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "type" ? Populate(new TypeElement(), element) : null;
        }

        public static TypeElement Populate(TypeElement target, XElement element)
        {
            AnyTypeElement.Populate(target, element);
            target.Documentation = Documentation.Load(element);
            target.Types = AnyTypeElement.LoadFrom(element).ToList();
            return target;
        }

        public Documentation Documentation { get; set; }

        public List<AnyTypeElement> Types { get; set; }

        public override string ToString()
        {
            return Name;
        }

    }

}
