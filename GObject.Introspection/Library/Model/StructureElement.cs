using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Library.Model
{

    public abstract class StructureElement : Element, IHasName, IHasCType, IHasInfo, IHasClrInfo
    {

        public static IEnumerable<StructureElement> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<StructureElement>();
        }

        public static StructureElement Load(XElement element)
        {
            return (StructureElement)ObjectElement.Load(element) ?? (StructureElement)RecordElement.Load(element);
        }

        public static StructureElement Populate(StructureElement target, XElement element)
        {
            Element.Populate(target, element);
            target.Info = Info.Load(element);
            target.Documentation = Documentation.Load(element);
            target.Annotations = AnnotationElement.LoadFrom(element).ToList();
            target.Name = (string)element.Attribute("name");
            target.CType = (string)element.Attribute(Xmlns.C_1_0_NS + "type");
            target.GLibTypeName = (string)element.Attribute(Xmlns.GLib_1_0_NS + "type-name");
            target.GLibGetType = (string)element.Attribute(Xmlns.GLib_1_0_NS + "get-type");
            target.CSymbolPrefix = (string)element.Attribute(Xmlns.C_1_0_NS + "symbol-prefix");
            target.Fields = FieldElement.LoadFrom(element).ToList();
            target.Constructors = ConstructorElement.LoadFrom(element).ToList();
            target.Functions = FunctionElement.LoadFrom(element).ToList();
            target.Methods = MethodElement.LoadFrom(element).ToList();
            target.ClrInfo = ClrInfo.Load(element);
            return target;
        }

        public Info Info { get; set; }

        public Documentation Documentation { get; set; }

        public List<AnnotationElement> Annotations { get; set; }

        public string Name { get; set; }

        public string CType { get; set; }

        public string GLibTypeName { get; set; }

        public string GLibGetType { get; set; }

        public string CSymbolPrefix { get; set; }

        public List<FieldElement> Fields { get; set; }

        public List<ConstructorElement> Constructors { get; set; }

        public List<FunctionElement> Functions { get; set; }

        public List<MethodElement> Methods { get; set; }

        public ClrInfo ClrInfo { get; set; }

    }

}
