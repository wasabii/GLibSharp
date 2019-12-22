using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Xml
{

    public class FieldElement : Element, IHasInfo
    {

        public static IEnumerable<FieldElement> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<FieldElement>();
        }

        public static FieldElement Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "field" ? Populate(new FieldElement(), element) : null;
        }

        public static FieldElement Populate(FieldElement target, XElement element)
        {
            Element.Populate(target, element);
            target.Info = Info.Load(element);
            target.Documentation = Documentation.Load(element);
            target.Annotations = AnnotationElement.LoadFrom(element).ToList();
            target.Name = (string)element.Attribute("name");
            target.Writable = element.Attribute("writable").ToBool();
            target.Readable = element.Attribute("readable").ToBool();
            target.Private = element.Attribute("private").ToBool();
            target.Bits = (int?)element.Attribute("bits");
            target.Type = AnyTypeElement.LoadFrom(element).FirstOrDefault();
            target.Callback = CallbackElement.LoadFrom(element).FirstOrDefault();
            return target;
        }

        public Info Info { get; set; }

        public Documentation Documentation { get; set; }

        public List<AnnotationElement> Annotations { get; set; }

        public string Name { get; set; }

        public bool? Writable { get; set; }

        public bool? Readable { get; set; }

        public bool? Private { get; set; }

        public int? Bits { get; set; }

        public AnyTypeElement Type { get; set; }

        public CallbackElement Callback { get; set; }

        public override string ToString()
        {
            return Name;
        }

    }

}
