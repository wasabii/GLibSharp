using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Gir.Xml
{

    public class Property : IHasInfo
    {

        public static IEnumerable<Property> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<Property>();
        }

        public static Property Load(XElement element)
        {
            if (element.Name == Xmlns.Core_1_0_NS + "property")
                return Populate(new Property(), element);

            return null;
        }

        public static Property Populate(Property target, XElement element)
        {
            target.Info = Info.Load(element);
            target.Documentation = Documentation.Load(element);
            target.Annotations = Annotation.LoadFrom(element).ToList();
            target.Name = (string)element.Attribute("name");
            target.Writable = (int?)element.Attribute("writable") == 1;
            target.Readable = (int?)element.Attribute("readable") == 1;
            target.Construct = (int?)element.Attribute("construct") == 1;
            target.ConstructOnly = (int?)element.Attribute("construct-only") == 1;
            target.TransferOwnership = XmlUtil.ParseEnum<TransferOwnership>((string)element.Attribute("transfer-ownership"));
            target.Type = AnyType.LoadFrom(element).FirstOrDefault();
            return target;
        }

        public Info Info { get; set; }

        public Documentation Documentation { get; set; }

        public List<Annotation> Annotations { get; set; }

        public string Name { get; set; }

        public bool Writable { get; set; }

        public bool Readable { get; set; }

        public bool Construct { get; set; }

        public bool ConstructOnly { get; set; }

        public TransferOwnership? TransferOwnership { get; set; }

        public AnyType Type { get; set; }

        public override string ToString()
        {
            return Name;
        }

    }

}
