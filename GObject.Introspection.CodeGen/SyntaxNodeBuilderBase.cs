using System.Collections.Generic;

using Microsoft.CodeAnalysis;

namespace GObject.Introspection.CodeGen
{

    /// <summary>
    /// Implements the base class for building a syntax node.
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    abstract partial class SyntaxNodeBuilderBase<T> : SyntaxNodeBuilderBase
    {

        /// <summary>
        /// Builds the syntax for the specified object.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        protected abstract IEnumerable<SyntaxNode> Build(T source);

    }

    /// <summary>
    /// Implements the base class for building a syntax node.
    /// </summary>
    abstract partial class SyntaxNodeBuilderBase
    {



    }

}
