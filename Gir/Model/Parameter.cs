using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Gir.Model
{

    public class Parameter : ParameterBase
    {

        public static new IEnumerable<Parameter> LoadFrom(XContainer container)
        {
            return ParameterBase.LoadFrom(container).OfType<Parameter>();
        }

        public static new Parameter Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "parameter" ? Populate(new Parameter(), element) : null;
        }

        public static Parameter Populate(Parameter target, XElement element)
        {
            ParameterBase.Populate(target, element);
            target.Introspectable = element.Attribute("introspectable").ToBool();
            target.Closure = (int?)element.Attribute("closure");
            target.Destroy = (int?)element.Attribute("destroy");
            target.Scope = element.Attribute("scope").ToEnum<ValueScope>();
            target.Optional = element.Attribute("optional").ToBool();
            target.Skip = element.Attribute("skip").ToBool();
            target.Type = AnyType.LoadFrom(element).FirstOrDefault();
            target.VarArgs = element.Elements(Xmlns.Core_1_0_NS + "varargs").Any();
            return target;
        }

        public bool? Introspectable { get; set; }

        public int? Closure { get; set; }

        public int? Destroy { get; set; }

        public ValueScope? Scope { get; set; }

        public bool? Optional { get; set; }

        public bool? Skip { get; set; }

        public AnyType Type { get; set; }

        public bool VarArgs { get; set; }

    }

}
