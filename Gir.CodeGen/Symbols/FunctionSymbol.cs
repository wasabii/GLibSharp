using System;

using Gir.Xml;

namespace Gir.CodeGen.Symbols
{

    class FunctionSymbol : ISymbol
    {

        readonly ISymbolResolver resolver;
        readonly string ns;
        readonly Function function;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        /// <param name="ns"></param>
        /// <param name="function"></param>
        public FunctionSymbol(ISymbolResolver resolver, string ns, Function function)
        {
            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            this.ns = ns ?? throw new ArgumentNullException(nameof(ns));
            this.function = function ?? throw new ArgumentNullException(nameof(function));
        }

        public SymbolName Name => new SymbolName(ns, function.Name);

    }

}
