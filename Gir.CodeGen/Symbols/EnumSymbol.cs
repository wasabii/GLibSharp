using System;

using Gir.Xml;

namespace Gir.CodeGen.Symbols
{

    class EnumSymbol : ISymbol
    {

        readonly ISymbolResolver resolver;
        readonly string ns;
        readonly Gir.Xml.Enum @enum;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        /// <param name="ns"></param>
        /// <param name="enum"></param>
        public EnumSymbol(ISymbolResolver resolver, string ns, Gir.Xml.Enum @enum)
        {
            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            this.ns = ns ?? throw new ArgumentNullException(nameof(ns));
            this.@enum = @enum ?? throw new ArgumentNullException(nameof(@enum));
        }

        public SymbolName Name => new SymbolName(ns, @enum.Name);

    }

}
