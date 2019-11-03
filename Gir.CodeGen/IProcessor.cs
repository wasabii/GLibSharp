using System.Collections.Generic;
using System.Xml.Linq;

using Microsoft.CodeAnalysis;

namespace Gir.CodeGen
{

    /// <summary>
    /// Provides processing of some element within a GIR.
    /// </summary>
    public interface IProcessor
    {

        /// <summary>
        /// Initiates a build for the given element.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        IEnumerable<SyntaxNode> Build(IContext context, XElement element);

        /// <summary>
        /// Initiates an adjustment for the syntax node returned by the builder for the given element.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="element"></param>
        /// <param name="initial"></param>
        /// <returns></returns>
        SyntaxNode Adjust(IContext context, XElement element, SyntaxNode initial);

    }

}