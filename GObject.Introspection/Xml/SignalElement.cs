using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Xml
{

    public class SignalElement : CallableElement, IHasInfo
    {

        public static new IEnumerable<SignalElement> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<SignalElement>();
        }

        public static new SignalElement Load(XElement element)
        {
            return element.Name == Xmlns.GLib_1_0_NS + "signal" ? Populate(new SignalElement(), element) : null;
        }

        public static SignalElement Populate(SignalElement target, XElement element)
        {
            CallableElement.Populate(target, element);
            target.Info = Info.Load(element);
            target.Documentation = Documentation.Load(element);
            target.Annotations = AnnotationElement.LoadFrom(element).ToList();
            target.Detailed = element.Attribute("detailed").ToBool();
            target.When = element.Attribute("when").ToEnum<When>();
            target.Action = element.Attribute("action").ToBool();
            target.NoHooks = element.Attribute("no-hooks").ToBool();
            target.NoRecurse = element.Attribute("no-recurse").ToBool();
            return target;
        }

        public Info Info { get; set; }

        public Documentation Documentation { get; set; }

        public List<AnnotationElement> Annotations { get; set; }

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
