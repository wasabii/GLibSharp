using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Gir.Xml
{

    public abstract class ParameterBase : IParameter
    {

        public static IEnumerable<ParameterBase> LoadFrom(XContainer container)
        {
            return container.Elements(Xmlns.Core_1_0_NS + "parameters").Elements().Select(i => Load(i)).OfType<ParameterBase>();
        }

        public static ParameterBase Load(XElement element)
        {
            return
                (ParameterBase)Parameter.Load(element) ??
                (ParameterBase)InstanceParameter.Load(element);
        }

        public static ParameterBase Populate(ParameterBase target, XElement element)
        {
            target.Documentation = Documentation.Load(element);
            target.Name = (string)element.Attribute("name");
            target.Nullable = (int?)element.Attribute("nullable") == 1;
            target.AllowNone = (int?)element.Attribute("allow-none") == 1;
            target.Direction = XmlUtil.ParseEnum<ParameterDirection>((string)element.Attribute("direction"));
            target.CallerAllocates = (int?)element.Attribute("caller-allocates") == 1;
            target.TransferOwnership = XmlUtil.ParseEnum<TransferOwnership>((string)element.Attribute("transfer-ownership"));
            return target;
        }

        public string Name { get; set; }

        public bool Nullable { get; set; }

        public bool AllowNone { get; set; }

        public ParameterDirection? Direction { get; set; }

        public bool CallerAllocates { get; set; }

        public TransferOwnership? TransferOwnership { get; set; }

        public Documentation Documentation { get; set; }

        public override string ToString()
        {
            return Name;
        }

    }

}
