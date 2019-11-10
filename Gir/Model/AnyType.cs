using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Gir.Model
{

    public abstract class AnyType : Element
    {

        public static IEnumerable<AnyType> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<AnyType>();
        }

        public static AnyType Load(XElement element)
        {
            return (AnyType)Type.Load(element) ?? (AnyType)ArrayType.Load(element);
        }

        public static AnyType Populate(AnyType target, XElement element)
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
