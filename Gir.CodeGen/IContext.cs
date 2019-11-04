using Microsoft.CodeAnalysis.Editing;

namespace Gir.CodeGen
{

    /// <summary>
    /// Provides information about the processing context.
    /// </summary>
    interface IContext
    {

        /// <summary>
        /// Gets the symbol resolver.
        /// </summary>
        ISymbolResolver Resolver { get; }

        /// <summary>
        /// Provides the current <see cref="SyntaxGenerator"/>.
        /// </summary>
        SyntaxGenerator Syntax { get; }

    }

}
