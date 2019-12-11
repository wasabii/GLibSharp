using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Model
{

    /// <summary>
    /// A simple type of data (as opposed to an array).
    /// </summary>
    public class Type : AnyType, IHasDocumentation
    {

        public static new IEnumerable<Type> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<Type>();
        }

        public static new Type Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "type" ? Populate(new Type(), element) : null;
        }

        public static Type Populate(Type target, XElement element)
        {
            AnyType.Populate(target, element);
            target.Documentation = Documentation.Load(element);
            target.Types = AnyType.LoadFrom(element).ToList();
            return target;
        }

        public Documentation Documentation { get; set; }

        public List<AnyType> Types { get; set; }

        public override string ToString()
        {
            return Name;
        }

    }

}
