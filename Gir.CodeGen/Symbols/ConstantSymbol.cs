using System;

using Gir.Xml;

namespace Gir.CodeGen.Symbols
{

    class ConstantSymbol : ISymbol
    {

        readonly ISymbolResolver resolver;
        readonly string ns;
        readonly Constant callback;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        /// <param name="ns"></param>
        /// <param name="constant"></param>
        public ConstantSymbol(ISymbolResolver resolver, string ns, Constant constant)
        {
            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            this.ns = ns ?? throw new ArgumentNullException(nameof(ns));
            this.callback = constant ?? throw new ArgumentNullException(nameof(constant));
        }

        public SymbolName Name => new SymbolName(ns, callback.Name);

    }

}
