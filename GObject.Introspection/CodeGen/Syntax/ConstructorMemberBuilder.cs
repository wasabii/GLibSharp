using System.Collections.Generic;

using GObject.Introspection.CodeGen.Model;

using Microsoft.CodeAnalysis;

namespace GObject.Introspection.CodeGen.Syntax
{

    /// <summary>
    /// Builds the syntax for method elements.
    /// </summary>
    class ConstructorMemberBuilder : MethodMemberBuilder<ConstructorMember>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="member"></param>
        public ConstructorMemberBuilder(ModuleContext context, ConstructorMember member) :
            base(context, member)
        {

        }

        public override IEnumerable<SyntaxNode> Build()
        {
            yield return
                Syntax.AddAttributes(
                    Syntax.ConstructorDeclaration(
                        Member.DeclaringType.Name,
                        BuildParameters(),
                        GetAccessibility(),
                        GetModifiers(),
                        BuildBaseArguments(),
                        BuildStatements()),
                    BuildAttributes())
                .NormalizeWhitespace();
        }

        IEnumerable<SyntaxNode> BuildBaseArguments()
        {
            yield break;
        }

    }

}
