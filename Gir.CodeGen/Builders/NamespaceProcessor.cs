using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using Microsoft.CodeAnalysis;

namespace Gir.CodeGen
{

    /// <summary>
    /// Builds the syntax for namespace elements.
    /// </summary>
    class NamespaceProcessor : ISyntaxNodeBuilder
    {

        public IEnumerable<SyntaxNode> Build(IContext context, XElement element)
        {
            if (element.Name == Xmlns.Core_1_0 + "namespace")
                yield return BuildNamespace(context, element, (string)element.Attribute("name"), (string)element.Attribute("version"));
        }

        SyntaxNode BuildNamespace(IContext context, XElement element, string name, string version) =>
            context.Syntax.NamespaceDeclaration(name,
                element.Elements().SelectMany(i => context.Build(i)))
            .NormalizeWhitespace();

        public SyntaxNode Adjust(IContext context, XElement element, SyntaxNode initial)
        {
            return initial;
        }

    }

}
