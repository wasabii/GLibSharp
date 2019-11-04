using System;
using System.Collections.Generic;
using System.Linq;

namespace Gir.CodeGen
{

    /// <summary>
    /// Provides resolution of symbols across multiple <see cref="ISymbolSource"/>s.
    /// </summary>
    class SymbolResolver : ISymbolResolver
    {

        readonly IEnumerable<ISymbolSource> sources;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="sources"></param>
        public SymbolResolver(IEnumerable<ISymbolSource> sources)
        {
            this.sources = sources ?? throw new ArgumentNullException(nameof(sources));
        }

        public IEnumerable<ISymbol> Resolve(string ns)
        {
            return sources.SelectMany(i => i.Resolve(ns));
        }

        public IEnumerable<ISymbol> Resolve(SymbolName name)
        {
            return sources.SelectMany(i => i.Resolve(name));
        }

    }

}
