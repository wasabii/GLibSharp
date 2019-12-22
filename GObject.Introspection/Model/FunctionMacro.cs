using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Model
{

    public class FunctionMacro : Element, IHasInfo, IHasName, IHasParameters
    {

        public static IEnumerable<FunctionMacro> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<FunctionMacro>();
        }

        public static FunctionMacro Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "function-macro" ? Populate(new FunctionMacro(), element) : null;
        }

        public static FunctionMacro Populate(FunctionMacro target, XElement element)
        {
            Element.Populate(target, element);
            target.Info = Info.Load(element);
            target.Documentation = Documentation.Load(element);
            target.Annotations = Annotation.LoadFrom(element).ToList();
            target.Name = (string)element.Attribute("name");
            target.Parameters = ParameterBase.LoadFrom(element).Cast<IParameter>().ToList();
            return target;
        }

        public Info Info { get; set; }

        public Documentation Documentation { get; set; }

        public List<Annotation> Annotations { get; set; }

        public string Name { get; set; }

        public List<IParameter> Parameters { get; set; }

    }

}
