using GObject.Introspection.CodeGen.Model.Expressions;

using Microsoft.CodeAnalysis;

namespace GObject.Introspection.CodeGen.Syntax
{

    class NotExpressionBuilder : SyntaxExpressionBuilderBase<NotExpression>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="expression"></param>
        public NotExpressionBuilder(ModuleContext context, NotExpression expression) :
            base(context, expression)
        {

        }

        public override SyntaxNode Build()
        {
            return Syntax.LogicalNotExpression(Context.Build(Expression.Expression));
        }

    }

}