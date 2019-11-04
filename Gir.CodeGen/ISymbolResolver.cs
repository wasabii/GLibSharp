using System.Collections.Generic;

namespace Gir.CodeGen
{

    /// <summary>
    /// Provides the ability to lookup specific qualified symbols.
    /// </summary>
    interface ISymbolResolver
    {

        /// <summary>
        /// Returns the <see cref="ISymbol"/> given the specified name.
        /// </summary>
        /// <param name="ns"></param>
        /// <returns></returns>
        IEnumerable<ISymbol> Resolve(string ns);

        /// <summary>
        /// Returns the <see cref="ISymbol"/> given the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IEnumerable<ISymbol> Resolve(SymbolName name);

    }

}
