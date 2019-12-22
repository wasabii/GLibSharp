using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Xml
{

    public class InstanceParameterElement : ParameterElementBase
    {

        public static new IEnumerable<InstanceParameterElement> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<InstanceParameterElement>();
        }

        public static new InstanceParameterElement Load(XElement element)
        {
            if (element.Name == Xmlns.Core_1_0_NS + "instance-parameter")
                return Populate(new InstanceParameterElement(), element);

            return null;
        }

        public static InstanceParameterElement Populate(InstanceParameterElement target, XElement element)
        {
            ParameterElementBase.Populate(target, element);
            target.Type = TypeElement.LoadFrom(element).FirstOrDefault();
            return target;
        }

        public TypeElement Type { get; set; }

    }

}
