using GObject.Introspection.CodeGen.Model.Expressions;

using Microsoft.CodeAnalysis;

namespace GObject.Introspection.CodeGen.Syntax
{

    class ConditionalExpressionBuilder : SyntaxExpressionBuilderBase<ConditionalExpression>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="expression"></param>
        public ConditionalExpressionBuilder(ModuleContext context, ConditionalExpression expression) :
            base(context, expression)
        {

        }

        public override SyntaxNode Build()
        {
            return Syntax.ConditionalExpression(
                Context.Build(Expression.If),
                Context.Build(Expression.Then),
                Context.Build(Expression.Else));
        }

    }

}