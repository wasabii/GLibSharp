using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Gir.Xml
{

    public class Signal : Callable, IHasInfo
    {

        public static new IEnumerable<Signal> LoadFrom(XContainer container)
        {
            return Callable.LoadFrom(container).OfType<Signal>();
        }

        public static new Signal Load(XElement element)
        {
            if (element.Name == Xmlns.GLib_1_0_NS + "signal")
                return Populate(new Signal(), element);

            return null;
        }

        public static Signal Populate(Signal target, XElement element)
        {
            Callable.Populate(target, element);
            target.Info = Info.Load(element);
            target.Documentation = Documentation.Load(element);
            target.Annotations = Annotation.LoadFrom(element).ToList();
            target.Name = (string)element.Attribute("name");
            target.Detailed = (int?)element.Attribute("detailed") == 1;
            target.When = XmlUtil.ParseEnum<When>((string)element.Attribute("when"));
            target.Action = (int?)element.Attribute("action") == 1;
            target.NoHooks = (int?)element.Attribute("no-hooks") == 1;
            target.NoRecurse = (int?)element.Attribute("no-recurse") == 1;
            return target;
        }

        public Info Info { get; set; }

        public Documentation Documentation { get; set; }

        public List<Annotation> Annotations { get; set; }

        public string Name { get; set; }

        public bool Detailed { get; set; }

        public When? When { get; set; }

        public bool Action { get; set; }

        public bool NoHooks { get; set; }

        public bool NoRecurse { get; set; }

        public override string ToString()
        {
            return Name;
        }

    }

}
