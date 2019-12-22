using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Xml
{

    public class ClassElement : ObjectElement, IHasInfo
    {

        public static new IEnumerable<ClassElement> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<ClassElement>();
        }

        public static new ClassElement Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "class" ? Populate(new ClassElement(), element) : null;
        }

        public static ClassElement Populate(ClassElement target, XElement element)
        {
            ObjectElement.Populate(target, element);
            target.Parent = (string)element.Attribute("parent");
            target.GLibTypeStruct = (string)element.Attribute(Xmlns.GLib_1_0_NS + "type-struct");
            target.GLibRefFunc = (string)element.Attribute(Xmlns.GLib_1_0_NS + "ref-func");
            target.GLibUnrefFunc = (string)element.Attribute(Xmlns.GLib_1_0_NS + "unref-func");
            target.GLibSetValueFunc = (string)element.Attribute(Xmlns.GLib_1_0_NS + "set-value-func");
            target.GLibGetValueFunc = (string)element.Attribute(Xmlns.GLib_1_0_NS + "get-value-func");
            target.Abstract = element.Attribute("abstract").ToBool();
            target.Fundamental = element.Attribute("fundamental").ToBool();
            target.Implements = GObject.Introspection.Xml.ImplementsElement.LoadFrom(element).ToList();
            target.VirtualMethods = VirtualMethodElement.LoadFrom(element).ToList();
            target.Properties = PropertyElement.LoadFrom(element).ToList();
            target.Signals = SignalElement.LoadFrom(element).ToList();
            target.Unions = UnionElement.LoadFrom(element).ToList();
            target.Constants = ConstantElement.LoadFrom(element).ToList();
            target.Records = RecordElement.LoadFrom(element).ToList();
            target.Callbacks = CallbackElement.LoadFrom(element).ToList();
            return target;
        }

        public string Parent { get; set; }

        public string GLibTypeStruct { get; set; }

        public string GLibRefFunc { get; set; }

        public string GLibUnrefFunc { get; set; }

        public string GLibSetValueFunc { get; set; }

        public string GLibGetValueFunc { get; set; }

        public bool? Abstract { get; set; }

        public bool? Fundamental { get; set; }

        public List<ImplementsElement> Implements { get; set; }

        public List<VirtualMethodElement> VirtualMethods { get; set; }

        public List<SignalElement> Signals { get; set; }

        public List<UnionElement> Unions { get; set; }

        public List<ConstantElement> Constants { get; set; }

        public List<RecordElement> Records { get; set; }

        public List<CallbackElement> Callbacks { get; set; }

        public override string ToString()
        {
            return Name ?? GLibTypeName ?? CType;
        }

    }

}
