using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Gir.Model
{

    public abstract class Structure : Element, IHasName, IHasInfo, IHasClrInfo
    {

        public static IEnumerable<Structure> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<Structure>();
        }

        public static Structure Load(XElement element)
        {
            return (Structure)Object.Load(element) ?? (Structure)Record.Load(element);
        }

        public static Structure Populate(Structure target, XElement element)
        {
            Element.Populate(target, element);
            target.Info = Info.Load(element);
            target.Documentation = Documentation.Load(element);
            target.Annotations = Annotation.LoadFrom(element).ToList();
            target.Name = (string)element.Attribute("name");
            target.CType = (string)element.Attribute(Xmlns.C_1_0_NS + "type");
            target.GLibTypeName = (string)element.Attribute(Xmlns.GLib_1_0_NS + "type-name");
            target.GLibGetType = (string)element.Attribute(Xmlns.GLib_1_0_NS + "get-type");
            target.CSymbolPrefix = (string)element.Attribute(Xmlns.C_1_0_NS + "symbol-prefix");
            target.Fields = Field.LoadFrom(element).ToList();
            target.Constructors = Constructor.LoadFrom(element).ToList();
            target.Functions = Function.LoadFrom(element).ToList();
            target.Methods = Method.LoadFrom(element).ToList();
            target.ClrInfo = ClrInfo.Load(element);
            return target;
        }

        public Info Info { get; set; }

        public Documentation Documentation { get; set; }

        public List<Annotation> Annotations { get; set; }

        public string Name { get; set; }

        public string CType { get; set; }

        public string GLibTypeName { get; set; }

        public string GLibGetType { get; set; }

        public string CSymbolPrefix { get; set; }

        public List<Field> Fields { get; set; }

        public List<Constructor> Constructors { get; set; }

        public List<Function> Functions { get; set; }

        public List<Method> Methods { get; set; }

        public ClrInfo ClrInfo { get; set; }

    }

}
