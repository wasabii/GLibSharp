using System;
using System.Collections.Generic;
using System.Xml.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace Gir.CodeGen
{

    /// <summary>
    /// Builds the syntax for class elements.
    /// </summary>
    class ClassProcessor : IProcessor
    {

        public IEnumerable<SyntaxNode> Build(IContext context, XElement element)
        {
            if (element.Name == Xmlns.Core_1_0 + "class")
                yield return BuildClass(context, element);
        }

        SyntaxNode BuildClass(IContext context, XElement element) =>
            context.Syntax.AddAttributes(
                context.Syntax.ClassDeclaration(
                    GetName(context, element),
                    GetTypeParameters(context, element),
                    GetAccessibility(context, element),
                    GetModifiers(context, element),
                    GetBaseType(context, element),
                    GetInterfaceTypes(context, element),
                    GetMembers(context, element)),
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

        IEnumerable<string> GetTypeParameters(IContext context, XElement element)
        {
            yield break;
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

        SyntaxNode? GetBaseType(IContext context, XElement element)
        {
            return default;
        }

        IEnumerable<SyntaxNode> GetInterfaceTypes(IContext context, XElement element)
        {
            yield break;
        }

        IEnumerable<SyntaxNode> GetMembers(IContext context, XElement element)
        {
            foreach (var i in element.Elements())
                foreach (var j in context.Build(i))
                    yield return j;
        }

        public SyntaxNode Adjust(IContext context, XElement element, SyntaxNode initial)
        {
            return initial;
        }

    }

}
