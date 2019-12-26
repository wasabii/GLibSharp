using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Library.Model
{

    public class InterfaceElement : Element, IHasName, IHasInfo, IHasClrInfo
    { 

        public static IEnumerable<InterfaceElement> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<InterfaceElement>();
        }

        public static InterfaceElement Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "interface" ? Populate(new InterfaceElement(), element) : null;
        }

        public static InterfaceElement Populate(InterfaceElement target, XElement element)
        {
            Element.Populate(target, element);
            target.Info = Info.Load(element);
            target.Documentation = Documentation.Load(element);
            target.Annotations = AnnotationElement.LoadFrom(element).ToList();
            target.Name = (string)element.Attribute("name");
            target.GLibTypeName = (string)element.Attribute(Xmlns.GLib_1_0_NS + "type-name");
            target.GLibGetType = (string)element.Attribute(Xmlns.GLib_1_0_NS + "get-type");
            target.CSymbolPrefix = (string)element.Attribute(Xmlns.C_1_0_NS + "symbol-prefix");
            target.CType = (string)element.Attribute(Xmlns.C_1_0_NS + "type");
            target.GLibTypeStruct = (string)element.Attribute(Xmlns.GLib_1_0_NS + "type-struct");
            target.Prerequisites = PrerequisiteElement.LoadFrom(element).ToList();
            target.Implements = GObject.Introspection.Library.Model.ImplementsElement.LoadFrom(element).ToList();
            target.Functions = FunctionElement.LoadFrom(element).ToList();
            target.Constructor = ConstructorElement.LoadFrom(element).FirstOrDefault();
            target.Methods = MethodElement.LoadFrom(element).ToList();
            target.VirtualMethods = VirtualMethodElement.LoadFrom(element).ToList();
            target.Fields = FieldElement.LoadFrom(element).ToList();
            target.Properties = PropertyElement.LoadFrom(element).ToList();
            target.Signals = SignalElement.LoadFrom(element).ToList();
            target.Callbacks = CallbackElement.LoadFrom(element).ToList();
            target.Constants = ConstantElement.LoadFrom(element).ToList();
            target.ClrInfo = ClrInfo.Load(element);
            return target;
        }

        public Info Info { get; set; }

        public Documentation Documentation { get; set; }

        public List<AnnotationElement> Annotations { get; set; }

        public string Name { get; set; }

        public string GLibTypeName { get; set; }

        public string GLibGetType { get; set; }

        public string CSymbolPrefix { get; set; }

        public string CType { get; set; }

        public string GLibTypeStruct { get; set; }

        public ClrInfo ClrInfo { get; set; }

        public List<PrerequisiteElement> Prerequisites { get; set; }

        public List<ImplementsElement> Implements { get; set; }

        public List<FunctionElement> Functions { get; set; }

        public ConstructorElement Constructor { get; set; }

        public List<MethodElement> Methods { get; set; }

        public List<VirtualMethodElement> VirtualMethods { get; set; }

        public List<FieldElement> Fields { get; set; }

        public List<PropertyElement> Properties { get; set; }

        public List<SignalElement> Signals { get; set; }

        public List<CallbackElement> Callbacks { get; set; }

        public List<ConstantElement> Constants { get; set; }

    }

}
