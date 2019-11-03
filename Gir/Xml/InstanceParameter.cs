using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Gir.Xml
{

    public class InstanceParameter : ParameterBase
    {

        public static new IEnumerable<InstanceParameter> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<InstanceParameter>();
        }

        public static new InstanceParameter Load(XElement element)
        {
            if (element.Name == Xmlns.Core_1_0_NS + "instance-parameter")
                return Populate(new InstanceParameter(), element);

            return null;
        }

        public static InstanceParameter Populate(InstanceParameter target, XElement element)
        {
            ParameterBase.Populate(target, element);
            target.Type = Type.LoadFrom(element).FirstOrDefault();
            return target;
        }

        public Type Type { get; set; }

    }

}
