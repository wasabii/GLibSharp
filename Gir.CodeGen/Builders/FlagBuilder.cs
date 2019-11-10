using System;
using System.Collections.Generic;
using System.Linq;

using Gir.Model;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace Gir.CodeGen
{

    /// <summary>
    /// Builds the syntax for class elements.
    /// </summary>
    abstract class FlagBuilder<TElement> : SyntaxNodeBuilderBase<TElement>
        where TElement : Flag
    {

        protected override IEnumerable<SyntaxNode> Build(IContext context, TElement flag)
        {
            yield return BuildEnum(context, flag);
        }

        protected virtual SyntaxNode BuildEnum(IContext context, TElement flag) =>
            context.Syntax.AddAttributes(
                context.Syntax.EnumDeclaration(
                    GetName(context, flag),
                    GetAccessibility(context, flag),
                    GetModifiers(context, flag),
                    BuildMembers(context, flag)),
                BuildAttributes(context, flag))
            .NormalizeWhitespace();

        string GetName(IContext context, TElement symbol)
        {
            return symbol.Name;
        }

        Accessibility GetAccessibility(IContext context, TElement flag)
        {
            return Accessibility.Public;
        }

        DeclarationModifiers GetModifiers(IContext context, TElement flag)
        {
            return DeclarationModifiers.Partial;
        }

        IEnumerable<SyntaxNode> BuildMembers(IContext context, TElement flag)
        {
            return flag.Members.SelectMany(i => BuildMember(context, flag, i));
        }

        IEnumerable<SyntaxNode> BuildMember(IContext context, TElement flag, Member member)
        {
            yield return context.Syntax.EnumMember(member.Name, ConvertValue(context.Syntax, member.Value));
        }

        SyntaxNode ConvertValue(SyntaxGenerator syntax, string value)
        {
            if (int.TryParse(value, out var i))
                return syntax.LiteralExpression(i);
            if (long.TryParse(value, out var l))
                return syntax.LiteralExpression(l);

            throw new InvalidOperationException();
        }

    }

}
