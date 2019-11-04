using System;

using Gir.Xml;

namespace Gir.CodeGen.Symbols
{

    class UnionSymbol : ISymbol
    {

        readonly ISymbolResolver resolver;
        readonly string ns;
        readonly Union union;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        /// <param name="ns"></param>
        /// <param name="union"></param>
        public UnionSymbol(ISymbolResolver resolver, string ns, Union union)
        {
            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            this.ns = ns ?? throw new ArgumentNullException(nameof(ns));
            this.union = union ?? throw new ArgumentNullException(nameof(union));
        }

        public SymbolName Name => new SymbolName(ns, union.Name);

    }

}
