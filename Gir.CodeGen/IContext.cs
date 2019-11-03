using System.Collections.Generic;
using System.Xml.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace Gir.CodeGen
{

    /// <summary>
    /// Provides information about the processing context.
    /// </summary>
    public interface IContext
    {

        /// <summary>
        /// Initiates a build for the given element from the given parent processor.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        IEnumerable<SyntaxNode> Build(XElement element);

        /// <summary>
        /// Resolves a namespace from the set of known repositories.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        XElement? ResolveNamespace(string name, string version);

        /// <summary>
        /// Provides the current <see cref="SyntaxGenerator"/>.
        /// </summary>
        SyntaxGenerator Syntax { get; }

    }

}
