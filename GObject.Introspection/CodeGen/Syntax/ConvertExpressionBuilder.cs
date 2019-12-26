using GObject.Introspection.CodeGen.Model.Expressions;

using Microsoft.CodeAnalysis;

namespace GObject.Introspection.CodeGen.Syntax
{

    class ConvertExpressionBuilder : SyntaxExpressionBuilderBase<ConvertExpression>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="expression"></param>
        public ConvertExpressionBuilder(ModuleContext context, ConvertExpression expression) :
            base(context, expression)
        {

        }

        public override SyntaxNode Build()
        {
            return Syntax.ConvertExpression(
                Syntax.TypeSymbol(Expression.Type),
                Context.Build(Expression.Value));
        }

    }

}