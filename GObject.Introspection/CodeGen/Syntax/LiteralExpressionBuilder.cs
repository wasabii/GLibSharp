using GObject.Introspection.CodeGen.Model.Expressions;

using Microsoft.CodeAnalysis;

namespace GObject.Introspection.CodeGen.Syntax
{

    class LiteralExpressionBuilder : SyntaxExpressionBuilderBase<LiteralExpression>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="expression"></param>
        public LiteralExpressionBuilder(ModuleContext context, LiteralExpression expression) :
            base(context, expression)
        {

        }

        public override SyntaxNode Build()
        {
            return Syntax.LiteralExpression(Expression.Value);
        }

    }

}