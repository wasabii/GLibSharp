using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Gir.Xml
{

    public class Enum : IHasInfo
    {

        public static IEnumerable<Enum> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<Enum>();
        }

        public static Enum Load(XElement element)
        {
            if (element.Name == Xmlns.Core_1_0_NS + "enum")
                return Populate(new Enum(), element);

            return null;
        }

        public static Enum Populate(Enum target, XElement element)
        {
            target.Info = Info.Load(element);
            target.Documentation = Documentation.Load(element);
            target.Annotations = Annotation.LoadFrom(element).ToList();
            target.Name = (string)element.Attribute("name");
            target.CType = (string)element.Attribute(Xmlns.C_1_0_NS + "type");
            target.GLibTypeName = (string)element.Attribute(Xmlns.GLib_1_0_NS + "type-name");
            target.GLibGetType = (string)element.Attribute(Xmlns.GLib_1_0_NS + "get-type");
            target.GLibErrorDomain = (string)element.Attribute(Xmlns.GLib_1_0_NS + "error-domain");
            target.Members = Member.LoadFrom(element).ToList();
            target.Functions = Function.LoadFrom(element).ToList();
            return target;
        }

        public Info Info { get; set; }

        public Documentation Documentation { get; set; }

        public List<Annotation> Annotations { get; set; }

        public string Name { get; set; }

        public string CType { get; set; }

        public string GLibTypeName { get; set; }

        public string GLibGetType { get; set; }

        public string GLibErrorDomain { get; set; }

        public List<Member> Members { get; set; }

        public List<Function> Functions { get; set; }

        public override string ToString()
        {
            return Name ?? GLibTypeName ?? CType;
        }

    }

}

