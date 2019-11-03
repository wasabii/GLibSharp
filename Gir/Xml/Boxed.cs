using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Gir.Xml
{

    /// <summary>
    /// Boxed type (wrapper to opaque C structures registered by the type system).
    /// </summary>
    public class Boxed : IHasInfo
    {

        public static IEnumerable<Boxed> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<Boxed>();
        }

        public static Boxed Load(XElement element)
        {
            if (element.Name == Xmlns.GLib_1_0_NS + "boxed")
                return Populate(new Boxed(), element);

            return null;
        }

        public static Boxed Populate(Boxed target, XElement element)
        {
            target.Info = Info.Load(element);
            target.Documentation = Documentation.Load(element);
            target.Annotations = Annotation.LoadFrom(element).ToList();
            target.GLibName = (string)element.Attribute(Xmlns.GLib_1_0_NS + "name");
            target.CSymbolPrefix = (string)element.Attribute(Xmlns.C_1_0_NS + "symbol-prefix");
            target.GLibTypeName = (string)element.Attribute(Xmlns.GLib_1_0_NS + "type-name");
            target.GLibGetType = (string)element.Attribute(Xmlns.GLib_1_0_NS + "get-type");
            target.Functions = Function.LoadFrom(element).ToList();
            return target;
        }

        public Info Info { get; set; }

        public Documentation Documentation { get; set; }

        public List<Annotation> Annotations { get; set; }

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

        public List<Function> Functions { get; set; }

        public override string ToString()
        {
            return GLibName ?? GLibTypeName;
        }

    }

}
