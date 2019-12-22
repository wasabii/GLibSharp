using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Xml
{

    /// <summary>
    /// Boxed type (wrapper to opaque C structures registered by the type system).
    /// </summary>
    public class BoxedElement : Element, IHasName, IHasInfo
    {

        public static IEnumerable<BoxedElement> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<BoxedElement>();
        }

        public static BoxedElement Load(XElement element)
        {
            return element.Name == Xmlns.GLib_1_0_NS + "boxed" ? Populate(new BoxedElement(), element) : null;
        }

        public static BoxedElement Populate(BoxedElement target, XElement element)
        {
            Element.Populate(target, element);
            target.Info = Info.Load(element);
            target.Documentation = Documentation.Load(element);
            target.Annotations = AnnotationElement.LoadFrom(element).ToList();
            target.GLibName = (string)element.Attribute(Xmlns.GLib_1_0_NS + "name");
            target.CSymbolPrefix = (string)element.Attribute(Xmlns.C_1_0_NS + "symbol-prefix");
            target.GLibTypeName = (string)element.Attribute(Xmlns.GLib_1_0_NS + "type-name");
            target.GLibGetType = (string)element.Attribute(Xmlns.GLib_1_0_NS + "get-type");
            target.Functions = FunctionElement.LoadFrom(element).ToList();
            return target;
        }

        public Info Info { get; set; }

        public Documentation Documentation { get; set; }

        public List<AnnotationElement> Annotations { get; set; }

        /// <summary>
        /// GObject compatible type name of the boxed type.
        /// </summary>
        public string GLibName { get; set; }

        /// <summary>
        /// Prefix to filter out from C functions. For example, gtk_window_new will lose gtk_.
        /// </summary>
        public string CSymbolPrefix { get; set; }

        /// <summary>
        /// GObject compatible type name of the boxed type.
        /// </summary>
        public string GLibTypeName { get; set; }

        /// <summary>
        /// Function to get the GObject compatible type of the boxed type.
        /// </summary>
        public string GLibGetType { get; set; }

        public List<FunctionElement> Functions { get; set; }

        string IHasName.Name => GLibTypeName;

        public override string ToString()
        {
            return GLibName ?? GLibTypeName;
        }

    }

}
