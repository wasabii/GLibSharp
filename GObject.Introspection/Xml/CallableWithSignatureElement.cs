using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Xml
{

    public abstract class CallableWithSignatureElement : CallableElement, IHasInfo
    {

        public static new IEnumerable<CallableWithSignatureElement> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<CallableWithSignatureElement>();
        }

        public static new CallableWithSignatureElement Load(XElement element)
        {
            return
                (CallableWithSignatureElement)ConstructorElement.Load(element) ??
                (CallableWithSignatureElement)FunctionElement.Load(element) ??
                (CallableWithSignatureElement)MethodElement.Load(element) ??
                (CallableWithSignatureElement)VirtualMethodElement.Load(element);
        }

        public static CallableWithSignatureElement Populate(CallableWithSignatureElement target, XElement element)
        {
            CallableElement.Populate(target, element);
            target.Info = Info.Load(element);
            target.Documentation = Documentation.Load(element);
            target.Annotations = AnnotationElement.LoadFrom(element).ToList();
            target.CIdentifier = (string)element.Attribute(Xmlns.C_1_0_NS + "identifier");
            target.ShadowedBy = (string)element.Attribute("shadowed-by");
            target.Shadows = (string)element.Attribute("shadows");
            target.MovedTo = (string)element.Attribute("moved-to");
            return target;
        }

        public Info Info { get; set; }

        public Documentation Documentation { get; set; }

        public List<AnnotationElement> Annotations { get; set; }

        public string CIdentifier { get; set; }

        public string ShadowedBy { get; set; }

        public string Shadows { get; set; }

        public string MovedTo { get; set; }

        public override string ToString()
        {
            return Name ?? CIdentifier;
        }

    }

}
