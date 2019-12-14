using System.Collections.Generic;

using GObject.Introspection.Reflection;

using Microsoft.CodeAnalysis;

namespace GObject.Introspection.CodeGen
{

    /// <summary>
    /// Provides processing of some element within a GIR.
    /// </summary>
    public interface ISyntaxNodeBuilder
    {

        /// <summary>
        /// Initiates a build for the given symbol.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        IEnumerable<SyntaxNode> Build(IIntrospectionNode node);

    }

}
