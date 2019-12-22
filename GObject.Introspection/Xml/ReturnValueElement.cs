using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Xml
{

    /// <summary>
    /// Return value of a callable.
    /// </summary>
    public class ReturnValueElement : Element, IHasDocumentation
    {

        public static IEnumerable<ReturnValueElement> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<ReturnValueElement>();
        }

        public static ReturnValueElement Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "return-value" ? Populate(new ReturnValueElement(), element) : null;
        }

        public static ReturnValueElement Populate(ReturnValueElement target, XElement element)
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
            target.Type = AnyTypeElement.LoadFrom(element).FirstOrDefault();
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

        public AnyTypeElement Type { get; set; }

        public override string ToString()
        {
            return Type?.ToString();
        }

    }

}
