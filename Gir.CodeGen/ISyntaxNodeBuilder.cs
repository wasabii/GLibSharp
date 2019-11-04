using System.Collections.Generic;

using Microsoft.CodeAnalysis;

namespace Gir.CodeGen
{

    /// <summary>
    /// Provides processing of some element within a GIR.
    /// </summary>
    interface ISyntaxNodeBuilder
    {

        /// <summary>
        /// Initiates a build for the given symbol.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        IEnumerable<SyntaxNode> Build(IContext context, ISymbol symbol);

        /// <summary>
        /// Initiates an adjustment for the syntax node returned by the builder for the given symbol.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="symbol"></param>
        /// <param name="initial"></param>
        /// <returns></returns>
        SyntaxNode Adjust(IContext context, ISymbol symbol, SyntaxNode initial);

    }

}
