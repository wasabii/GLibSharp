using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Model
{

    public class Constant : Element, IHasName, IHasInfo
    {

        public static IEnumerable<Constant> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<Constant>();
        }

        public static Constant Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "constant" ? Populate(new Constant(), element) : null;
        }

        public static Constant Populate(Constant target, XElement element)
        {
            Element.Populate(target, element);
            target.Info = Info.Load(element);
            target.Documentation = Documentation.Load(element);
            target.Annotations = Annotation.LoadFrom(element).ToList();
            target.Name = (string)element.Attribute("name");
            target.Value = (string)element.Attribute("value");
            target.CType = (string)element.Attribute(Xmlns.C_1_0_NS + "type");
            target.CIdentifier = (string)element.Attribute(Xmlns.C_1_0_NS + "identifier");
            target.Type = AnyType.LoadFrom(element).FirstOrDefault();
            return target;
        }

        public Info Info { get; set; }

        public Documentation Documentation { get; set; }

        public List<Annotation> Annotations { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public string CType { get; set; }

        public string CIdentifier { get; set; }

        public AnyType Type { get; set; }

        public override string ToString()
        {
            return Name ?? CIdentifier;
        }

    }

}
