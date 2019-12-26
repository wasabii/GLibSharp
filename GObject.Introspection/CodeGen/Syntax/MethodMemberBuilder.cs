using System;
using System.Collections.Generic;
using System.Linq;
using GObject.Introspection.CodeGen.Model;

using Microsoft.CodeAnalysis;

namespace GObject.Introspection.CodeGen.Syntax
{

    /// <summary>
    /// Builds the syntax for method elements.
    /// </summary>
    class MethodMemberBuilder : SyntaxMemberBuilderBase<MethodMember>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="member"></param>
        public MethodMemberBuilder(ModuleContext context, MethodMember member) :
            base(context, member)
        {

        }

        public override IEnumerable<SyntaxNode> Build()
        {
            yield return
                Syntax.AddAttributes(
                    Syntax.MethodDeclaration(
                        GetName(),
                        BuildParameters(),
                        BuildTypeParameters(),
                        BuildReturnType(),
                        GetAccessibility(),
                        GetModifiers(),
                        BuildStatements()),
                    BuildAttributes())
                .NormalizeWhitespace();
        }

        IEnumerable<SyntaxNode> BuildParameters()
        {
            // TODO should not really be allowed
            if (Member.Invokable == null)
                yield break;

            foreach (var arg in Member.Invokable.Arguments)
                yield return Syntax.ParameterDeclaration(arg.Name, Syntax.TypeSymbol(arg.Type));
        }

        IEnumerable<string> BuildTypeParameters()
        {
            yield break;
        }

        SyntaxNode BuildReturnType()
        {
            // TODO should not really be allowed
            if (Member.Invokable == null)
                return null;

            if (Member.Invokable.ReturnArgument == null)
                return Syntax.TypeExpression(SpecialType.System_Void);

            return Syntax.TypeSymbol(Member.Invokable.ReturnArgument.Type);
        }

        IEnumerable<SyntaxNode> BuildStatements()
        {
            // TODO should not really be allowed
            if (Member.Invokable == null)
                yield break;

            foreach (var statement in Member.Invokable.Statements)
                yield return Context.Build(statement);
        }

        IEnumerable<SyntaxNode> BuildAttributes()
        {
            yield break;
        }

    }

}
