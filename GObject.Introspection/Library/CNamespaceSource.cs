using System.Xml.Linq;

namespace GObject.Introspection.Library
{

    /// <summary>
    /// Provides well-known C type information.
    /// </summary>
    class CNamespaceSource : NamespaceXmlSource
    {

        readonly static XDocument xml = XDocument.Load(typeof(CNamespaceSource).Assembly.GetManifestResourceStream("GObject.Introspection.Library.C-1.0.gir"));

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public CNamespaceSource() :
            base(xml)
        {

        }

    }

}
