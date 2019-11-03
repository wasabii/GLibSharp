using System.Collections.Generic;
using System.Xml.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace Gir.CodeGen
{

    /// <summary>
    /// Builds the syntax for property elements.
    /// </summary>
    class PropertyProcessor : IProcessor
    {

        public IEnumerable<SyntaxNode> Build(IContext context, XElement element)
        {
            if (element.Name == Xmlns.Core_1_0 + "property")
                yield return BuildProperty(context, element);
        }

        SyntaxNode BuildProperty(IContext context, XElement element) =>
            context.Syntax.AddAttributes(
                context.Syntax.PropertyDeclaration(
                    GetName(context, element),
                    GetType(context, element),
                    GetAccessibility(context, element),
                    GetModifiers(context, element),
                    GetGetAccessorStatements(context, element),
                    GetSetAccessorStatements(context, element)),
                GetAttributes(context, element))
            .NormalizeWhitespace();

        IEnumerable<SyntaxNode> GetAttributes(IContext context, XElement element)
        {
            yield return context.Syntax.Attribute("Gir.Class",
                context.Syntax.AttributeArgument("name", context.Syntax.LiteralExpression((string)element.Attribute("name"))));

            if ((string)element.Attribute("deprecated") == "1")
                yield return context.Syntax.Attribute("Obsolete");
        }

        string GetName(IContext context, XElement element)
        {
            return (string)element.Attribute("name");
        }

        SyntaxNode GetType(IContext context, XElement element)
        {
            return context.Syntax.IdentifierName("sometype");
        }

        Accessibility GetAccessibility(IContext context, XElement element)
        {
            if ((string)element.Attribute("internal") != "1")
                return Accessibility.Public;
            else
                return Accessibility.Internal;
        }

        DeclarationModifiers GetModifiers(IContext context, XElement element)
        {
            return DeclarationModifiers.Partial;
        }

        IEnumerable<SyntaxNode> GetGetAccessorStatements(IContext context, XElement element)
        {
            yield break;
        }

        IEnumerable<SyntaxNode> GetSetAccessorStatements(IContext context, XElement element)
        {
            yield break;
        }

        public SyntaxNode Adjust(IContext context, XElement element, SyntaxNode initial)
        {
            return initial;
        }

    }

}
