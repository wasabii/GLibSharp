using System.Collections.Generic;

using GObject.Introspection.CodeGen.Model;

using Microsoft.CodeAnalysis;

namespace GObject.Introspection.CodeGen.Syntax
{

    /// <summary>
    /// Builds the syntax for enum members.
    /// </summary>
    class EnumMemberBuilder : SyntaxMemberBuilderBase<EnumMember>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="member"></param>
        public EnumMemberBuilder(ModuleContext context, EnumMember member) :
            base(context, member)
        {

        }

        public override IEnumerable<SyntaxNode> Build()
        {
            yield return Syntax.AddAttributes(
                Syntax.EnumMember(
                    GetName(),
                    BuildExpression()),
                BuildAttributes())
            .NormalizeWhitespace();
        }

        SyntaxNode BuildExpression()
        {
            return Syntax.LiteralExpression(Member.Value);
        }

        IEnumerable<SyntaxNode> BuildAttributes()
        {
            yield break;
        }

    }

}
