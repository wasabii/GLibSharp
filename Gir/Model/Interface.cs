using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Gir.Model
{

    public class Interface : Element, IHasName, IHasInfo
    { 

        public static IEnumerable<Interface> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<Interface>();
        }

        public static Interface Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "interface" ? Populate(new Interface(), element) : null;
        }

        public static Interface Populate(Interface target, XElement element)
        {
            Element.Populate(target, element);
            target.Info = Info.Load(element);
            target.Documentation = Documentation.Load(element);
            target.Annotations = Annotation.LoadFrom(element).ToList();
            target.Name = (string)element.Attribute("name");
            target.GLibTypeName = (string)element.Attribute(Xmlns.GLib_1_0_NS + "type-name");
            target.GLibGetType = (string)element.Attribute(Xmlns.GLib_1_0_NS + "get-type");
            target.CSymbolPrefix = (string)element.Attribute(Xmlns.C_1_0_NS + "symbol-prefix");
            target.CType = (string)element.Attribute(Xmlns.C_1_0_NS + "type");
            target.GLibTypeStruct = (string)element.Attribute(Xmlns.GLib_1_0_NS + "type-struct");
            target.Prerequisites = Prerequisite.LoadFrom(element).ToList();
            target.Implements = Gir.Model.Implements.LoadFrom(element).ToList();
            target.Functions = Function.LoadFrom(element).ToList();
            target.Constructor = Constructor.LoadFrom(element).FirstOrDefault();
            target.Methods = Method.LoadFrom(element).ToList();
            target.VirtualMethods = VirtualMethod.LoadFrom(element).ToList();
            target.Fields = Field.LoadFrom(element).ToList();
            target.Properties = Property.LoadFrom(element).ToList();
            target.Signals = Signal.LoadFrom(element).ToList();
            target.Callbacks = Callback.LoadFrom(element).ToList();
            target.Constants = Constant.LoadFrom(element).ToList();
            return target;
        }

        public Info Info { get; set; }

        public Documentation Documentation { get; set; }

        public List<Annotation> Annotations { get; set; }

        public string Name { get; set; }

        public string GLibTypeName { get; set; }

        public string GLibGetType { get; set; }

        public string CSymbolPrefix { get; set; }

        public string CType { get; set; }

        public string GLibTypeStruct { get; set; }

        public List<Prerequisite> Prerequisites { get; set; }

        public List<Implements> Implements { get; set; }

        public List<Function> Functions { get; set; }

        public Constructor Constructor { get; set; }

        public List<Method> Methods { get; set; }

        public List<VirtualMethod> VirtualMethods { get; set; }

        public List<Field> Fields { get; set; }

        public List<Property> Properties { get; set; }

        public List<Signal> Signals { get; set; }

        public List<Callback> Callbacks { get; set; }

        public List<Constant> Constants { get; set; }

    }

}
