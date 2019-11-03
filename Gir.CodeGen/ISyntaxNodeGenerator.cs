using System.Collections.Generic;

using Microsoft.CodeAnalysis;

namespace Gir.CodeGen
{

    public interface ISyntaxNodeGenerator
    {

        /// <summary>
        /// Builds the <see cref="SyntaxNode"/> for a given namespace.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        IEnumerable<SyntaxNode> BuildNamespace(IContext context, string name, string version);

    }

}
