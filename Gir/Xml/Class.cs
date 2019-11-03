using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Gir.Xml
{

    public class Class : IHasInfo
    {

        public static IEnumerable<Class> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<Class>();
        }

        public static Class Load(XElement element)
        {
            if (element.Name == Xmlns.Core_1_0_NS + "class")
                return Populate(new Class(), element);

            return null;
        }

        public static Class Populate(Class target, XElement element)
        {
            target.Info = Info.Load(element);
            target.Documentation = Documentation.Load(element);
            target.Annotations = Annotation.LoadFrom(element).ToList();
            target.Name = (string)element.Attribute("name");
            target.GLibTypeName = (string)element.Attribute(Xmlns.GLib_1_0_NS + "type-name");
            target.GLibGetType = (string)element.Attribute(Xmlns.GLib_1_0_NS + "get-type");
            target.Parent = (string)element.Attribute("parent");
            target.GLibTypeStruct = (string)element.Attribute(Xmlns.GLib_1_0_NS + "type-struct");
            target.GLibRefFunc = (string)element.Attribute(Xmlns.GLib_1_0_NS + "ref-func");
            target.GLibUnrefFunc = (string)element.Attribute(Xmlns.GLib_1_0_NS + "unref-func");
            target.GLibSetValueFunc = (string)element.Attribute(Xmlns.GLib_1_0_NS + "set-value-func");
            target.GLibGetValueFunc = (string)element.Attribute(Xmlns.GLib_1_0_NS + "get-value-func");
            target.CType = (string)element.Attribute(Xmlns.C_1_0_NS + "type");
            target.CSymbolPrefix = (string)element.Attribute(Xmlns.C_1_0_NS + "symbol-prefix");
            target.Abstract = (int?)element.Attribute("abstract") == 1;
            target.Fundamental = (int?)element.Attribute("fundamental") == 1;
            target.Implements = Gir.Xml.Implements.LoadFrom(element).ToList();
            target.Constructors = Constructor.LoadFrom(element).ToList();
            target.Methods = Method.LoadFrom(element).ToList();
            target.Functions = Function.LoadFrom(element).ToList();
            target.VirtualMethods = VirtualMethod.LoadFrom(element).ToList();
            target.Fields = Field.LoadFrom(element).ToList();
            target.Properties = Property.LoadFrom(element).ToList();
            target.Signals = Signal.LoadFrom(element).ToList();
            target.Unions = Union.LoadFrom(element).ToList();
            target.Constants = Constant.LoadFrom(element).ToList();
            target.Records = Record.LoadFrom(element).ToList();
            target.Callbacks = Callback.LoadFrom(element).ToList();
            return target;
        }

        public Info Info { get; set; }

        public Documentation Documentation { get; set; }

        public List<Annotation> Annotations { get; set; }

        public string Name { get; set; }

        public string GLibTypeName { get; set; }

        public string GLibGetType { get; set; }

        public string Parent { get; set; }

        public string GLibTypeStruct { get; set; }

        public string GLibRefFunc { get; set; }

        public string GLibUnrefFunc { get; set; }

        public string GLibSetValueFunc { get; set; }

        public string GLibGetValueFunc { get; set; }

        public string CType { get; set; }

        public string CSymbolPrefix { get; set; }

        public bool Abstract { get; set; }

        public bool Fundamental { get; set; }

        public List<Implements> Implements { get; set; }

        public List<Constructor> Constructors { get; set; }

        public List<Method> Methods { get; set; }

        public List<Function> Functions { get; set; }

        public List<VirtualMethod> VirtualMethods { get; set; }

        public List<Field> Fields { get; set; }

        public List<Property> Properties { get; set; }

        public List<Signal> Signals { get; set; }

        public List<Union> Unions { get; set; }

        public List<Constant> Constants { get; set; }

        public List<Record> Records { get; set; }

        public List<Callback> Callbacks { get; set; }

        public override string ToString()
        {
            return Name ?? GLibTypeName ?? CType;
        }

    }

}
