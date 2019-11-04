using System;

using Gir.Xml;

namespace Gir.CodeGen.Symbols
{

    class RecordSymbol : ISymbol
    {

        readonly ISymbolResolver resolver;
        readonly string ns;
        readonly Record record;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        /// <param name="ns"></param>
        /// <param name="record"></param>
        public RecordSymbol(ISymbolResolver resolver, string ns, Record record)
        {
            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            this.ns = ns ?? throw new ArgumentNullException(nameof(ns));
            this.record = record ?? throw new ArgumentNullException(nameof(record));
        }

        public SymbolName Name => new SymbolName(ns, record.Name);

    }

}
