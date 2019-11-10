using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Gir.Model
{

    /// <summary>
    /// Return value of a callable.
    /// </summary>
    public class ReturnValue : Element, IHasDocumentation
    {

        public static IEnumerable<ReturnValue> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<ReturnValue>();
        }

        public static ReturnValue Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "return-value" ? Populate(new ReturnValue(), element) : null;
        }

        public static ReturnValue Populate(ReturnValue target, XElement element)
        {
            Element.Populate(target, element);
            target.Introspectable = (int?)element.Attribute("introspectable") != 0;
            target.Nullable = element.Attribute("nullable").ToBool();
            target.Closure = (int?)element.Attribute("closure");
            target.Scope = element.Attribute("scope").ToEnum<ValueScope>();
            target.Destroy = (int?)element.Attribute("destroy");
            target.Skip = element.Attribute("skip").ToBool();
            target.AllowNone = element.Attribute("allow-none").ToBool();
            target.TransferOwnership = element.Attribute("transfer-ownership").ToEnum<TransferOwnership>();
            target.Documentation = Documentation.Load(element);
            target.Type = AnyType.LoadFrom(element).FirstOrDefault();
            return target;
        }

        public bool? Introspectable { get; set; }

        public bool? Nullable { get; set; }

        public int? Closure { get; set; }

        public ValueScope? Scope { get; set; }

        public int? Destroy { get; set; }

        public bool? Skip { get; set; }

        public bool? AllowNone { get; set; }

        public TransferOwnership? TransferOwnership { get; set; }

        public Documentation Documentation { get; set; }

        public AnyType Type { get; set; }

        public override string ToString()
        {
            return Type?.ToString();
        }

    }

}
