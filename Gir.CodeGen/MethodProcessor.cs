using System;
using System.Collections.Generic;
using System.Xml.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace Gir.CodeGen
{

    /// <summary>
    /// Builds the syntax for method elements.
    /// </summary>
    class MethodProcessor : IProcessor
    {

        public IEnumerable<SyntaxNode> Build(IContext context, XElement element)
        {
            if (element.Name == Xmlns.Core_1_0 + "method")
                yield return BuildClass(context, element);
        }

        SyntaxNode BuildClass(IContext context, XElement element) =>
            context.Syntax.MethodDeclaration(
                GetName(context, element),
                GetParameters(context, element),
                GetTypeParameters(context, element),
                GetReturnType(context, element),
                GetAccessibility(context, element),
                GetModifiers(context, element),
                GetStatements(context, element))
            .NormalizeWhitespace();

        string GetName(IContext context, XElement element)
        {
            return (string)element.Attribute(Xmlns.C_1_0 + "identifier");
        }

        IEnumerable<SyntaxNode> GetParameters(IContext context, XElement element)
        {
            yield break;
        }

        IEnumerable<string> GetTypeParameters(IContext context, XElement element)
        {
            yield break;
        }

        SyntaxNode? GetReturnType(IContext context, XElement element)
        {
            return null;
        }

        Accessibility GetAccessibility(IContext context, XElement element)
        {
            return Accessibility.Private;
        }

        DeclarationModifiers GetModifiers(IContext context, XElement element)
        {
            return DeclarationModifiers.None;
        }

        IEnumerable<SyntaxNode> GetStatements(IContext context, XElement element)
        {
            yield break;
        }

        public SyntaxNode Adjust(IContext context, XElement element, SyntaxNode initial)
        {
            return initial;
        }

    }

}
