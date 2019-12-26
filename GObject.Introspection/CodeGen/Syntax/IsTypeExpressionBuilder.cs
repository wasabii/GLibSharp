using GObject.Introspection.CodeGen.Model.Expressions;

using Microsoft.CodeAnalysis;

namespace GObject.Introspection.CodeGen.Syntax
{

    class IsTypeExpressionBuilder : SyntaxExpressionBuilderBase<IsTypeExpression>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="expression"></param>
        public IsTypeExpressionBuilder(ModuleContext context, IsTypeExpression expression) :
            base(context, expression)
        {

        }

        public override SyntaxNode Build()
        {
            return Syntax.IsTypeExpression(Context.Build(Expression.Expression), Syntax.TypeSymbol(Expression.IsType));
        }

    }

}