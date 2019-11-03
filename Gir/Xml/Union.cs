using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Gir.Xml
{

    public class Union
    {

        public static IEnumerable<Union> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<Union>();
        }

        public static Union Load(XElement element)
        {
            if (element.Name == Xmlns.Core_1_0_NS + "union")
                return Populate(new Union(), element);

            return null;
        }

        public static Union Populate(Union target, XElement element)
        {
            target.Info = Info.Load(element);
            target.Name = (string)element.Attribute("name");
            target.CType = (string)element.Attribute(Xmlns.C_1_0_NS + "type");
            target.CSymbolPrefix = (string)element.Attribute(Xmlns.C_1_0_NS + "symbol-prefix");
            target.GLibTypeName = (string)element.Attribute(Xmlns.GLib_1_0_NS + "type-name");
            target.GLibGetType = (string)element.Attribute(Xmlns.GLib_1_0_NS + "get-type");
            target.Fields = Field.LoadFrom(element).ToList();
            target.Constructors = Constructor.LoadFrom(element).ToList();
            target.Methods = Method.LoadFrom(element).ToList();
            target.Functions = Function.LoadFrom(element).ToList();
            target.Records = Record.LoadFrom(element).ToList();
            return target;
        }

        public Info Info { get; set; }

        public string Name { get; set; }

        public string CType { get; set; }

        public string CSymbolPrefix { get; set; }

        public string GLibTypeName { get; set; }

        public string GLibGetType { get; set; }

        public List<Field> Fields { get; set; }

        public List<Constructor> Constructors { get; set; }

        public List<Method> Methods { get; set; }

        public List<Function> Functions { get; set; }

        public List<Record> Records { get; set; }

        public override string ToString()
        {
            return Name ?? GLibTypeName ?? CType;
        }

    }

}
