using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Gir.Model
{

    /// <summary>
    /// Type's name substitution, representing a typedef in C.
    /// </summary>
    public class Alias : Element, IHasName, IHasInfo
    {

        public static IEnumerable<Alias> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<Alias>();
        }

        public static Alias Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "alias" ? Populate(new Alias(), element) : null;
        }

        public static Alias Populate(Alias target, XElement element)
        {
            Element.Populate(target, element);
            target.Info = Info.Load(element);
            target.Documentation = Documentation.Load(element);
            target.Annotations = Annotation.LoadFrom(element).ToList();
            target.Name = (string)element.Attribute("name");
            target.CType = (string)element.Attribute(Xmlns.Core_1_0_NS + "type");
            target.Type = Type.LoadFrom(element).FirstOrDefault();
            return target;
        }

        public Info Info { get; set; }

        public Documentation Documentation { get; set; }

        public List<Annotation> Annotations { get; set; }

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
        public Type Type { get; set; }

        public override string ToString()
        {
            return Name ?? CType;
        }

    }

}
