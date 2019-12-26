using GObject.Introspection.CodeGen.Model;
using GObject.Introspection.CodeGen.Model.Expressions;

using Microsoft.CodeAnalysis;

namespace GObject.Introspection.CodeGen.Syntax
{

    class ParameterExpressionBuilder : SyntaxExpressionBuilderBase<ParameterExpression>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="expression"></param>
        public ParameterExpressionBuilder(ModuleContext context, ParameterExpression expression) :
            base(context, expression)
        {

        }

        public override SyntaxNode Build()
        {
            if (Expression.Argument is ThisArgument self)
                return Syntax.ThisExpression();
            else
                return Syntax.IdentifierName(Expression.Name);
        }

    }

}
