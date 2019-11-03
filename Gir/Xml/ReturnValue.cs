using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Gir.Xml
{

    /// <summary>
    /// Return value of a callable.
    /// </summary>
    public class ReturnValue : IHasDocumentation
    {

        public static IEnumerable<ReturnValue> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<ReturnValue>();
        }

        public static ReturnValue Load(XElement element)
        {
            if (element.Name == Xmlns.Core_1_0_NS + "return-value")
                return Populate(new ReturnValue(), element);

            return null;
        }

        public static ReturnValue Populate(ReturnValue target, XElement element)
        {
            target.Introspectable = (int?)element.Attribute("introspectable") != 0;
            target.Nullable = (int?)element.Attribute("nullable") == 1;
            target.Closure = (int?)element.Attribute("closure");
            target.Scope = XmlUtil.ParseEnum<ValueScope>((string)element.Attribute("scope"));
            target.Destroy = (int?)element.Attribute("destroy");
            target.Skip = (int?)element.Attribute("skip") == 1;
            target.AllowNone = (int?)element.Attribute("allow-none") == 1;
            target.TransferOwnership = XmlUtil.ParseEnum<TransferOwnership>((string)element.Attribute("transfer-ownership")); 
            target.Documentation = Documentation.Load(element);
            target.Type = AnyType.LoadFrom(element).FirstOrDefault();
            return target;
        }

        public bool Introspectable { get; set; }

        public bool Nullable { get; set; }

        public int? Closure { get; set; }

        public ValueScope? Scope { get; set; }

        public int? Destroy { get; set; }

        public bool Skip { get; set; }

        public bool AllowNone { get; set; }

        public TransferOwnership? TransferOwnership { get; set; }

        public Documentation Documentation { get; set; }

        public AnyType Type { get; set; }

        public override string ToString()
        {
            return Type?.ToString();
        }

    }

}
