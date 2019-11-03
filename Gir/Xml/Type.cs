using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Gir.Xml
{

    /// <summary>
    /// A simple type of data (as opposed to an array).
    /// </summary>
    public class Type : AnyType, IHasDocumentation
    {

        public static new IEnumerable<Type> LoadFrom(XContainer container)
        {
            return AnyType.LoadFrom(container).OfType<Type>();
        }

        public static new Type Load(XElement element)
        {
            if (element.Name == Xmlns.Core_1_0_NS + "type")
                return Populate(new Type(), element);

            return null;
        }

        public static Type Populate(Type target, XElement element)
        {
            target.Name = (string)element.Attribute("name");
            target.CType = (string)element.Attribute(Xmlns.C_1_0_NS + "type");
            target.Introspectable = (int?)element.Attribute("introspectable") == 1;
            target.Documentation = Documentation.Load(element);
            target.Types = AnyType.LoadFrom(element).ToList();
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

        public Documentation Documentation { get; set; }

        public List<AnyType> Types { get; set; }

        public override string ToString()
        {
            return Name;
        }

    }

}
