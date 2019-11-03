using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Gir.Xml
{

    public abstract class CallableWithSignature : Callable, IHasInfo
    {

        public static new IEnumerable<CallableWithSignature> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<CallableWithSignature>();
        }

        public static new CallableWithSignature Load(XElement element)
        {
            return
                (CallableWithSignature)Constructor.Load(element) ??
                (CallableWithSignature)Function.Load(element) ??
                (CallableWithSignature)Method.Load(element) ??
                (CallableWithSignature)VirtualMethod.Load(element);
        }

        public static CallableWithSignature Populate(CallableWithSignature target, XElement element)
        {
            Callable.Populate(target, element);
            target.Info = Info.Load(element);
            target.Documentation = Documentation.Load(element);
            target.Annotations = Annotation.LoadFrom(element).ToList();
            target.Name = (string)element.Attribute("name");
            target.CIdentifier = (string)element.Attribute(Xmlns.C_1_0_NS + "identifier");
            target.ShadowedBy = (string)element.Attribute("shadowed-by");
            target.Shadows = (string)element.Attribute("shadows");
            target.Throws = (int?)element.Attribute("throws") == 1;
            target.MovedTo = (string)element.Attribute("moved-to");
            return target;
        }

        public Info Info { get; set; }

        public Documentation Documentation { get; set; }

        public List<Annotation> Annotations { get; set; }

        public string Name { get; set; }

        public string CIdentifier { get; set; }

        public string ShadowedBy { get; set; }

        public string Shadows { get; set; }

        public bool Throws { get; set; }

        public string MovedTo { get; set; }

        public override string ToString()
        {
            return Name ?? CIdentifier;
        }

    }

}
