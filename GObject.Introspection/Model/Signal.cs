using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Model
{

    public class Signal : Callable, IHasInfo
    {

        public static new IEnumerable<Signal> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<Signal>();
        }

        public static new Signal Load(XElement element)
        {
            return element.Name == Xmlns.GLib_1_0_NS + "signal" ? Populate(new Signal(), element) : null;
        }

        public static Signal Populate(Signal target, XElement element)
        {
            Callable.Populate(target, element);
            target.Info = Info.Load(element);
            target.Documentation = Documentation.Load(element);
            target.Annotations = Annotation.LoadFrom(element).ToList();
            target.Detailed = element.Attribute("detailed").ToBool();
            target.When = element.Attribute("when").ToEnum<When>();
            target.Action = element.Attribute("action").ToBool();
            target.NoHooks = element.Attribute("no-hooks").ToBool();
            target.NoRecurse = element.Attribute("no-recurse").ToBool();
            return target;
        }

        public Info Info { get; set; }

        public Documentation Documentation { get; set; }

        public List<Annotation> Annotations { get; set; }

        public bool? Detailed { get; set; }

        public When? When { get; set; }

        public bool? Action { get; set; }

        public bool? NoHooks { get; set; }

        public bool? NoRecurse { get; set; }

        public override string ToString()
        {
            return Name;
        }

    }

}
