using System;

using Gir.Xml;

namespace Gir.CodeGen.Symbols
{

    class InterfaceSymbol : ISymbol
    {

        readonly ISymbolResolver resolver;
        readonly string ns;
        readonly Interface iface;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        /// <param name="ns"></param>
        /// <param name="iface"></param>
        public InterfaceSymbol(ISymbolResolver resolver, string ns, Interface iface)
        {
            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            this.ns = ns ?? throw new ArgumentNullException(nameof(ns));
            this.iface = iface ?? throw new ArgumentNullException(nameof(iface));
        }

        public SymbolName Name => new SymbolName(ns, iface.Name);

    }

}
