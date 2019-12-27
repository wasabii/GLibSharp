using GObject.Introspection.CodeGen.Model.Expressions;

using Microsoft.CodeAnalysis;

namespace GObject.Introspection.CodeGen.Syntax
{

    class AssignExpressionBuilder : SyntaxExpressionBuilderBase<AssignExpression>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="expression"></param>
        public AssignExpressionBuilder(ModuleContext context, AssignExpression expression) :
            base(context, expression)
        {

        }

        public override SyntaxNode Build()
        {
            // assembly what should appear on the left of the assignment
            var left = Context.Build(Expression.Target);
            if (Expression.Member != null)
                left = Syntax.MemberAccessExpression(left, Expression.Member);

            // final assignment
            return Syntax.AssignmentStatement(left, Context.Build(Expression.Value));
        }

    }

}