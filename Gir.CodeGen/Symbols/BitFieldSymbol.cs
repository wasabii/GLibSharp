using System;

using Gir.Xml;

namespace Gir.CodeGen.Symbols
{

    class BitFieldSymbol : ISymbol
    {

        readonly ISymbolResolver resolver;
        readonly string ns;
        readonly BitField bitfield;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        /// <param name="ns"></param>
        /// <param name="bitfield"></param>
        public BitFieldSymbol(ISymbolResolver resolver, string ns, BitField bitfield)
        {
            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            this.ns = ns ?? throw new ArgumentNullException(nameof(ns));
            this.bitfield = bitfield ?? throw new ArgumentNullException(nameof(bitfield));
        }

        public SymbolName Name => new SymbolName(ns, bitfield.Name);

    }

}
