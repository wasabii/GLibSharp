using System.Collections.Generic;

using Gir.CodeGen.Symbols;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace Gir.CodeGen
{

    /// <summary>
    /// Builds the syntax for class elements.
    /// </summary>
    class ClassBuilder : SyntaxNodeBuilderBase<ClassSymbol>
    {

        protected override IEnumerable<SyntaxNode> Build(IContext context, ClassSymbol symbol)
        {
            yield return BuildClass(context, symbol);
        }

        SyntaxNode BuildClass(IContext context, ClassSymbol symbol) =>
            context.Syntax.AddAttributes(
                context.Syntax.ClassDeclaration(
                    GetName(context, symbol),
                    GetTypeParameters(context, symbol),
                    GetAccessibility(context, symbol),
                    GetModifiers(context, symbol),
                    GetBaseType(context, symbol),
                    GetInterfaceTypes(context, symbol),
                    GetMembers(context, symbol)),
                GetAttributes(context, symbol))
            .NormalizeWhitespace();

        IEnumerable<SyntaxNode> GetAttributes(IContext context, ClassSymbol symbol)
        {
            yield return context.Syntax.Attribute("Gir.Class", context.Syntax.AttributeArgument(context.Syntax.LiteralExpression(symbol.Name.Name)));

            if (symbol.Deprecated)
                yield return context.Syntax.Attribute("Obsolete");
        }

        string GetName(IContext context, ClassSymbol symbol)
        {
            return symbol.Name.Name;
        }

        IEnumerable<string> GetTypeParameters(IContext context, ClassSymbol symbol)
        {
            yield break;
        }

        Accessibility GetAccessibility(IContext context, ClassSymbol symbol)
        {
            return Accessibility.Public;
        }

        DeclarationModifiers GetModifiers(IContext context, ClassSymbol symbol)
        {
            return DeclarationModifiers.Partial;
        }

        SyntaxNode? GetBaseType(IContext context, ClassSymbol symbol)
        {
            return default;
        }

        IEnumerable<SyntaxNode> GetInterfaceTypes(IContext context, ClassSymbol symbol)
        {
            yield break;
        }

        IEnumerable<SyntaxNode> GetMembers(IContext context, ClassSymbol symbol)
        {
            yield break;
        }

    }

}
