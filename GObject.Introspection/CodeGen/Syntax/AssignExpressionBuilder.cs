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
            return Syntax.AssignmentStatement(
                Syntax.IdentifierName(Expression.Variable.Name),
                Context.Build(Expression.Value));
        }

    }

}