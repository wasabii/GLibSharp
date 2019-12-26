using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Library.Model
{

    public abstract class ParameterElementBase : Element, IParameter
    {

        public static IEnumerable<ParameterElementBase> LoadFrom(XContainer container)
        {
            return container.Elements(Xmlns.Core_1_0_NS + "parameters").Elements().Select(i => Load(i)).OfType<ParameterElementBase>();
        }

        public static ParameterElementBase Load(XElement element)
        {
            return (ParameterElementBase)ParameterElement.Load(element) ?? (ParameterElementBase)InstanceParameterElement.Load(element);
        }

        public static ParameterElementBase Populate(ParameterElementBase target, XElement element)
        {
            Element.Populate(target, element);
            target.Documentation = Documentation.Load(element);
            target.Name = (string)element.Attribute("name");
            target.Nullable = element.Attribute("nullable").ToBool();
            target.AllowNone = element.Attribute("allow-none").ToBool();
            target.Direction = element.Attribute("direction").ToEnum<ParameterDirection>();
            target.CallerAllocates = element.Attribute("caller-allocates").ToBool();
            target.TransferOwnership = element.Attribute("transfer-ownership").ToEnum<TransferOwnership>();
            return target;
        }

        public string Name { get; set; }

        public bool? Nullable { get; set; }

        public bool? AllowNone { get; set; }

        public ParameterDirection? Direction { get; set; }

        public bool? CallerAllocates { get; set; }

        public TransferOwnership? TransferOwnership { get; set; }

        public Documentation Documentation { get; set; }

        public override string ToString()
        {
            return Name;
        }

    }

}
