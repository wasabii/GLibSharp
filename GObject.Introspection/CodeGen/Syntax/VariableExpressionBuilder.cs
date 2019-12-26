
using GObject.Introspection.CodeGen.Model.Expressions;

using Microsoft.CodeAnalysis;

namespace GObject.Introspection.CodeGen.Syntax
{

    class VariableExpressionBuilder : SyntaxExpressionBuilderBase<VariableExpression>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="expression"></param>
        public VariableExpressionBuilder(ModuleContext context, VariableExpression expression) :
            base(context, expression)
        {

        }

        public override SyntaxNode Build()
        {
            return Syntax.IdentifierName(Expression.Name);
        }

    }

}
