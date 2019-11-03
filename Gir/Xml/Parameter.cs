using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Gir.Xml
{

    public class Parameter : ParameterBase
    {

        public static new IEnumerable<Parameter> LoadFrom(XContainer container)
        {
            return ParameterBase.LoadFrom(container).OfType<Parameter>();
        }

        public static new Parameter Load(XElement element)
        {
            if (element.Name == Xmlns.Core_1_0_NS + "parameter")
                return Populate(new Parameter(), element);

            return null;
        }

        public static Parameter Populate(Parameter target, XElement element)
        {
            ParameterBase.Populate(target, element);
            target.Introspectable = (int?)element.Attribute("introspectable") != 0;
            target.Closure = (int?)element.Attribute("closure");
            target.Destroy = (int?)element.Attribute("destroy");
            target.Scope = XmlUtil.ParseEnum<ValueScope>((string)element.Attribute("scope"));
            target.Optional = (int?)element.Attribute("optional") == 1;
            target.Skip = (int?)element.Attribute("skip") == 1;
            target.Type = AnyType.LoadFrom(element).FirstOrDefault();
            target.VarArgs = element.Elements("varargs").Any();
            return target;
        }

        public bool Introspectable { get; set; }

        public int? Closure { get; set; }

        public int? Destroy { get; set; }

        public ValueScope? Scope { get; set; }

        public bool Optional { get; set; }

        public bool Skip { get; set; }

        public AnyType Type { get; set; }

        public bool VarArgs { get; set; }

    }

}
