using System.Xml.Linq;

namespace GObject.Introspection.Library
{

    /// <summary>
    /// Provides well-known C type information.
    /// </summary>
    public class BaseNamespaceSource : NamespaceXmlSource
    {

        readonly static XDocument xml = XDocument.Load(typeof(BaseNamespaceSource).Assembly.GetManifestResourceStream("GObject.Introspection.C-1.0.gir"));

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public BaseNamespaceSource() :
            base(xml)
        {

        }

    }

}
