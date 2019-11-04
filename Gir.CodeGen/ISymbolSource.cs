using System.Collections.Generic;

namespace Gir.CodeGen
{

    /// <summary>
    /// Provides a mechanism to resolve a symbol from a repository.
    /// </summary>
    public interface ISymbolSource
    {

        /// <summary>
        /// Resolves symbols within the specified namespace.
        /// </summary>
        /// <param name="ns"></param>
        /// <returns></returns>
        IEnumerable<ISymbol> Resolve(string ns);

        /// <summary>
        /// Retrieves symbols with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IEnumerable<ISymbol> Resolve(SymbolName name);

    }

}
