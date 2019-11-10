using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Gir.Model
{

    public class Field : Element, IHasInfo
    {

        public static IEnumerable<Field> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<Field>();
        }

        public static Field Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "field" ? Populate(new Field(), element) : null;
        }

        public static Field Populate(Field target, XElement element)
        {
            Element.Populate(target, element);
            target.Info = Info.Load(element);
            target.Documentation = Documentation.Load(element);
            target.Annotations = Annotation.LoadFrom(element).ToList();
            target.Name = (string)element.Attribute("name");
            target.Writable = element.Attribute("writable").ToBool();
            target.Readable = element.Attribute("readable").ToBool();
            target.Private = element.Attribute("private").ToBool();
            target.Bits = (int?)element.Attribute("bits");
            target.Type = AnyType.LoadFrom(element).FirstOrDefault();
            target.Callback = Callback.LoadFrom(element).FirstOrDefault();
            return target;
        }

        public Info Info { get; set; }

        public Documentation Documentation { get; set; }

        public List<Annotation> Annotations { get; set; }

        public string Name { get; set; }

        public bool? Writable { get; set; }

        public bool? Readable { get; set; }

        public bool? Private { get; set; }

        public int? Bits { get; set; }

        public AnyType Type { get; set; }

        public Callback Callback { get; set; }

        public override string ToString()
        {
            return Name;
        }

    }

}
