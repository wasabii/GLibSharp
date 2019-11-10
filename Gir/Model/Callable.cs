using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Gir.Model
{

    public abstract class Callable : Element, IHasName, IHasParameters, IHasReturnValue
    {

        public static IEnumerable<Callable> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<Callable>();
        }

        public static Callable Load(XElement element)
        {
            return (Callable)CallableWithSignature.Load(element) ?? (Callable)Callback.Load(element);
        }

        public static Callable Populate(Callable target, XElement element)
        {
            Element.Populate(target, element);
            target.Name = (string)element.Attribute("name");
            target.Parameters = ParameterBase.LoadFrom(element).Cast<IParameter>().ToList();
            target.ReturnValue = ReturnValue.LoadFrom(element).FirstOrDefault();
            return target;
        }

        public string Name { get; set; }

        public List<IParameter> Parameters { get; set; }

        public ReturnValue ReturnValue { get; set; }

    }

}
