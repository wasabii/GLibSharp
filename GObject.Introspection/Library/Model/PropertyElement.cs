using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Library.Model
{

    public class PropertyElement : Element, IHasInfo
    {

        public static IEnumerable<PropertyElement> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<PropertyElement>();
        }

        public static PropertyElement Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "property" ? Populate(new PropertyElement(), element) : null;
        }

        public static PropertyElement Populate(PropertyElement target, XElement element)
        {
            Element.Populate(target, element);
            target.Info = Info.Load(element);
            target.Documentation = Documentation.Load(element);
            target.Annotations = AnnotationElement.LoadFrom(element).ToList();
            target.Name = (string)element.Attribute("name");
            target.Writable = element.Attribute("writable").ToBool();
            target.Readable = element.Attribute("readable").ToBool();
            target.Construct = element.Attribute("construct").ToBool();
            target.ConstructOnly = element.Attribute("construct-only").ToBool();
            target.TransferOwnership = element.Attribute("transfer-ownership").ToEnum<TransferOwnership>();
            target.Type = AnyTypeElement.LoadFrom(element).FirstOrDefault();
            return target;
        }

        public Info Info { get; set; }

        public Documentation Documentation { get; set; }

        public List<AnnotationElement> Annotations { get; set; }

        public string Name { get; set; }

        public bool? Writable { get; set; }

        public bool? Readable { get; set; }

        public bool? Construct { get; set; }

        public bool? ConstructOnly { get; set; }

        public TransferOwnership? TransferOwnership { get; set; }

        public AnyTypeElement Type { get; set; }

        public override string ToString()
        {
            return Name;
        }

    }

}
