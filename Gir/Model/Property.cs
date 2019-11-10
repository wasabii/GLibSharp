using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Gir.Model
{

    public class Property : Element, IHasInfo
    {

        public static IEnumerable<Property> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<Property>();
        }

        public static Property Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "property" ? Populate(new Property(), element) : null;
        }

        public static Property Populate(Property target, XElement element)
        {
            Element.Populate(target, element);
            target.Info = Info.Load(element);
            target.Documentation = Documentation.Load(element);
            target.Annotations = Annotation.LoadFrom(element).ToList();
            target.Name = (string)element.Attribute("name");
            target.Writable = element.Attribute("writable").ToBool();
            target.Readable = element.Attribute("readable").ToBool();
            target.Construct = element.Attribute("construct").ToBool();
            target.ConstructOnly = element.Attribute("construct-only").ToBool();
            target.TransferOwnership = element.Attribute("transfer-ownership").ToEnum<TransferOwnership>();
            target.Type = AnyType.LoadFrom(element).FirstOrDefault();
            return target;
        }

        public Info Info { get; set; }

        public Documentation Documentation { get; set; }

        public List<Annotation> Annotations { get; set; }

        public string Name { get; set; }

        public bool? Writable { get; set; }

        public bool? Readable { get; set; }

        public bool? Construct { get; set; }

        public bool? ConstructOnly { get; set; }

        public TransferOwnership? TransferOwnership { get; set; }

        public AnyType Type { get; set; }

        public override string ToString()
        {
            return Name;
        }

    }

}
