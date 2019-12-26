using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Library.Model
{

    public abstract class CallableElement : Element, IHasName, IHasParameters, IHasReturnValue
    {

        public static IEnumerable<CallableElement> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<CallableElement>();
        }

        public static CallableElement Load(XElement element)
        {
            return (CallableElement)CallableWithSignatureElement.Load(element) ?? (CallableElement)CallbackElement.Load(element);
        }

        public static CallableElement Populate(CallableElement target, XElement element)
        {
            Element.Populate(target, element);
            target.Name = (string)element.Attribute("name");
            target.Parameters = ParameterElementBase.LoadFrom(element).Cast<IParameter>().ToList();
            target.ReturnValue = ReturnValueElement.LoadFrom(element).FirstOrDefault();
            return target;
        }

        public string Name { get; set; }

        public List<IParameter> Parameters { get; set; }

        public ReturnValueElement ReturnValue { get; set; }

    }

}
