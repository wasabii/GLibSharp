using System.Collections.Generic;

using GObject.Introspection.CodeGen.Model;

using Microsoft.CodeAnalysis;

namespace GObject.Introspection.CodeGen.Syntax
{

    /// <summary>
    /// Builds the syntax for method elements.
    /// </summary>
    class MethodMemberBuilder : MethodMemberBuilder<MethodMember>
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

    }

    /// <summary>
    /// Builds the syntax for method elements.
    /// </summary>
    abstract class MethodMemberBuilder<TMember> : SyntaxMemberBuilderBase<TMember>
        where TMember : MethodMember
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="member"></param>
        public MethodMemberBuilder(ModuleContext context, TMember member) :
            base(context, member)
        {

        }

        protected IEnumerable<SyntaxNode> BuildParameters()
        {
            // TODO should not really be allowed
            if (Member.Invokable == null)
                yield break;

            foreach (var arg in Member.Invokable.Parameters)
                yield return Syntax.ParameterDeclaration(arg.Name, Syntax.TypeSymbol(arg.Type));
        }

        protected IEnumerable<string> BuildTypeParameters()
        {
            yield break;
        }

        protected SyntaxNode BuildReturnType()
        {
            // TODO should not really be allowed
            if (Member.Invokable == null)
                return null;

            // method has no return type
            if (Member.Invokable.ReturnType == null)
                return null;

            return Syntax.TypeSymbol(Member.Invokable.ReturnType);
        }

        protected IEnumerable<SyntaxNode> BuildStatements()
        {
            // TODO should not really be allowed
            if (Member.Invokable == null)
                yield break;

            foreach (var statement in Member.Invokable.Statements)
                yield return Context.Build(statement);
        }

        protected IEnumerable<SyntaxNode> BuildAttributes()
        {
            yield break;
        }

    }

}
