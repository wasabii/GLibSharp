using System;

using Gir.Xml;

namespace Gir.CodeGen.Symbols
{

    class CallbackSymbol : ISymbol
    {

        readonly ISymbolResolver resolver;
        readonly string ns;
        readonly Callback callback;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        /// <param name="ns"></param>
        /// <param name="callback"></param>
        public CallbackSymbol(ISymbolResolver resolver, string ns, Callback callback)
        {
            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            this.ns = ns ?? throw new ArgumentNullException(nameof(ns));
            this.callback = callback ?? throw new ArgumentNullException(nameof(callback));
        }

        public SymbolName Name => new SymbolName(ns, callback.Name);

    }

}
