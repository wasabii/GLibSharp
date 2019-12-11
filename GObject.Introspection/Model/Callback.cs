using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Model
{

    public class Callback : Callable, IHasInfo, IHasParameters, IHasReturnValue, IHasClrInfo
    {

        public static new IEnumerable<Callback> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<Callback>();
        }

        public static new Callback Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "callback" ? Populate(new Callback(), element) : null;
        }

        public static Callback Populate(Callback target, XElement element)
        {
            Callable.Populate(target, element);
            target.Documentation = Documentation.Load(element);
            target.Info = Info.Load(element);
            target.CType = (string)element.Attribute(Xmlns.C_1_0_NS + "type");
            target.Throws = element.Attribute("throws").ToBool();
            target.ClrInfo = ClrInfo.Load(element);
            return target;
        }

        public Info Info { get; set; }

        public Documentation Documentation { get; set; }

        public List<Annotation> Annotations { get; set; }

        public string CType { get; set; }

        public bool? Throws { get; set; }

        public ClrInfo ClrInfo { get; set; }

        public override string ToString()
        {
            return Name ?? CType;
        }

    }

}
