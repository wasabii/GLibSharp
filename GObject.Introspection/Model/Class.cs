using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Model
{

    public class Class : Object, IHasInfo
    {

        public static new IEnumerable<Class> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<Class>();
        }

        public static new Class Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "class" ? Populate(new Class(), element) : null;
        }

        public static Class Populate(Class target, XElement element)
        {
            Object.Populate(target, element);
            target.Parent = (string)element.Attribute("parent");
            target.GLibTypeStruct = (string)element.Attribute(Xmlns.GLib_1_0_NS + "type-struct");
            target.GLibRefFunc = (string)element.Attribute(Xmlns.GLib_1_0_NS + "ref-func");
            target.GLibUnrefFunc = (string)element.Attribute(Xmlns.GLib_1_0_NS + "unref-func");
            target.GLibSetValueFunc = (string)element.Attribute(Xmlns.GLib_1_0_NS + "set-value-func");
            target.GLibGetValueFunc = (string)element.Attribute(Xmlns.GLib_1_0_NS + "get-value-func");
            target.Abstract = element.Attribute("abstract").ToBool();
            target.Fundamental = element.Attribute("fundamental").ToBool();
            target.Implements = GObject.Introspection.Model.Implements.LoadFrom(element).ToList();
            target.VirtualMethods = VirtualMethod.LoadFrom(element).ToList();
            target.Properties = Property.LoadFrom(element).ToList();
            target.Signals = Signal.LoadFrom(element).ToList();
            target.Unions = Union.LoadFrom(element).ToList();
            target.Constants = Constant.LoadFrom(element).ToList();
            target.Records = Record.LoadFrom(element).ToList();
            target.Callbacks = Callback.LoadFrom(element).ToList();
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

        public List<Implements> Implements { get; set; }

        public List<VirtualMethod> VirtualMethods { get; set; }

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
