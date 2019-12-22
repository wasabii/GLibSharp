using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Xml
{

    public class FunctionMacroElement : Element, IHasInfo, IHasName, IHasParameters
    {

        public static IEnumerable<FunctionMacroElement> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<FunctionMacroElement>();
        }

        public static FunctionMacroElement Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "function-macro" ? Populate(new FunctionMacroElement(), element) : null;
        }

        public static FunctionMacroElement Populate(FunctionMacroElement target, XElement element)
        {
            Element.Populate(target, element);
            target.Info = Info.Load(element);
            target.Documentation = Documentation.Load(element);
            target.Annotations = AnnotationElement.LoadFrom(element).ToList();
            target.Name = (string)element.Attribute("name");
            target.Parameters = ParameterElementBase.LoadFrom(element).Cast<IParameter>().ToList();
            return target;
        }

        public Info Info { get; set; }

        public Documentation Documentation { get; set; }

        public List<AnnotationElement> Annotations { get; set; }

        public string Name { get; set; }

        public List<IParameter> Parameters { get; set; }

    }

}
