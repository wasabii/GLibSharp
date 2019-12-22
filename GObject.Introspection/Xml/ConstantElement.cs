using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Xml
{

    public class ConstantElement : Element, IHasName, IHasInfo
    {

        public static IEnumerable<ConstantElement> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<ConstantElement>();
        }

        public static ConstantElement Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "constant" ? Populate(new ConstantElement(), element) : null;
        }

        public static ConstantElement Populate(ConstantElement target, XElement element)
        {
            Element.Populate(target, element);
            target.Info = Info.Load(element);
            target.Documentation = Documentation.Load(element);
            target.Annotations = AnnotationElement.LoadFrom(element).ToList();
            target.Name = (string)element.Attribute("name");
            target.Value = (string)element.Attribute("value");
            target.CType = (string)element.Attribute(Xmlns.C_1_0_NS + "type");
            target.CIdentifier = (string)element.Attribute(Xmlns.C_1_0_NS + "identifier");
            target.Type = AnyTypeElement.LoadFrom(element).FirstOrDefault();
            return target;
        }

        public Info Info { get; set; }

        public Documentation Documentation { get; set; }

        public List<AnnotationElement> Annotations { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public string CType { get; set; }

        public string CIdentifier { get; set; }

        public AnyTypeElement Type { get; set; }

        public override string ToString()
        {
            return Name ?? CIdentifier;
        }

    }

}
