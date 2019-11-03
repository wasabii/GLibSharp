using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Gir.Xml
{

    public class Record : IHasInfo
    {

        public static IEnumerable<Record> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<Record>();
        }

        public static Record Load(XElement element)
        {
            if (element.Name == Xmlns.Core_1_0_NS + "record")
                return Populate(new Record(), element);

            return null;
        }

        public static Record Populate(Record target, XElement element)
        {
            target.Info = Info.Load(element);
            target.Documentation = Documentation.Load(element);
            target.Annotations = Annotation.LoadFrom(element).ToList();
            target.Name = (string)element.Attribute("name");
            target.CType = (string)element.Attribute(Xmlns.C_1_0_NS + "type");
            target.Disguised = (int?)element.Attribute("disguised") == 1;
            target.GLibTypeName = (string)element.Attribute(Xmlns.GLib_1_0_NS + "type-name");
            target.GLibGetType = (string)element.Attribute(Xmlns.GLib_1_0_NS + "get-type");
            target.CSymbolPrefix = (string)element.Attribute(Xmlns.C_1_0_NS + "symbol-prefix");
            target.Foreign = (int?)element.Attribute("foreign") == 1;
            target.GLibIsGTypeStructFor = (string)element.Attribute(Xmlns.GLib_1_0_NS + "is-gtype-struct-for");
            target.Fields = Field.LoadFrom(element).ToList();
            target.Functions = Function.LoadFrom(element).ToList();
            target.Unions = Union.LoadFrom(element).ToList();
            target.Methods = Method.LoadFrom(element).ToList();
            target.Constructors = Constructor.LoadFrom(element).ToList();
            target.Properties = Property.LoadFrom(element).ToList();
            return target;
        }

        public Info Info { get; set; }

        public Documentation Documentation { get; set; }

        public List<Annotation> Annotations { get; set; }

        public string Name { get; set; }

        public string CType { get; set; }

        public bool Disguised { get; set; }

        public string GLibTypeName { get; set; }

        public string GLibGetType { get; set; }

        public string CSymbolPrefix { get; set; }

        public bool Foreign { get; set; }

        public string GLibIsGTypeStructFor { get; set; }

        public List<Field> Fields { get; set; }

        public List<Function> Functions { get; set; }

        public List<Union> Unions { get; set; }

        public List<Method> Methods { get; set; }

        public List<Constructor> Constructors { get; set; }

        public List<Property> Properties { get; set; }

        public override string ToString()
        {
            return Name ?? GLibTypeName ?? CType;
        }

    }

}
