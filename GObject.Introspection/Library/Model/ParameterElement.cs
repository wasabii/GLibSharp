using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Library.Model
{

    public class ParameterElement : ParameterElementBase
    {

        public static new IEnumerable<ParameterElement> LoadFrom(XContainer container)
        {
            return ParameterElementBase.LoadFrom(container).OfType<ParameterElement>();
        }

        public static new ParameterElement Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "parameter" ? Populate(new ParameterElement(), element) : null;
        }

        public static ParameterElement Populate(ParameterElement target, XElement element)
        {
            ParameterElementBase.Populate(target, element);
            target.Introspectable = element.Attribute("introspectable").ToBool();
            target.Closure = (int?)element.Attribute("closure");
            target.Destroy = (int?)element.Attribute("destroy");
            target.Scope = element.Attribute("scope").ToEnum<ValueScope>();
            target.Optional = element.Attribute("optional").ToBool();
            target.Skip = element.Attribute("skip").ToBool();
            target.Type = AnyTypeElement.LoadFrom(element).FirstOrDefault();
            target.VarArgs = element.Elements(Xmlns.Core_1_0_NS + "varargs").Any();
            return target;
        }

        public bool? Introspectable { get; set; }

        public int? Closure { get; set; }

        public int? Destroy { get; set; }

        public ValueScope? Scope { get; set; }

        public bool? Optional { get; set; }

        public bool? Skip { get; set; }

        public AnyTypeElement Type { get; set; }

        public bool VarArgs { get; set; }

    }

}
