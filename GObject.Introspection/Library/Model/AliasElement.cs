using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Library.Model
{

    /// <summary>
    /// Type's name substitution, representing a typedef in C.
    /// </summary>
    public class AliasElement : Element, IHasName, IHasInfo, IHasClrInfo
    {

        public static IEnumerable<AliasElement> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<AliasElement>();
        }

        public static AliasElement Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "alias" ? Populate(new AliasElement(), element) : null;
        }

        public static AliasElement Populate(AliasElement target, XElement element)
        {
            Element.Populate(target, element);
            target.Info = Info.Load(element);
            target.Documentation = Documentation.Load(element);
            target.Annotations = AnnotationElement.LoadFrom(element).ToList();
            target.Name = (string)element.Attribute("name");
            target.CType = (string)element.Attribute(Xmlns.Core_1_0_NS + "type");
            target.Type = TypeElement.LoadFrom(element).FirstOrDefault();
            target.ClrInfo = ClrInfo.Load(element);
            return target;
        }

        public Info Info { get; set; }

        public Documentation Documentation { get; set; }

        public List<AnnotationElement> Annotations { get; set; }

        /// <summary>
        /// The new name or typedef'd name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The corresponding C type's name.
        /// </summary>
        public string CType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TypeElement Type { get; set; }

        public ClrInfo ClrInfo { get; set; }

        public override string ToString()
        {
            return Name ?? CType;
        }

    }

}
