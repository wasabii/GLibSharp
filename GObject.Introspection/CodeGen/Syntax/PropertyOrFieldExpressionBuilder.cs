
using GObject.Introspection.CodeGen.Model.Expressions;

using Microsoft.CodeAnalysis;

namespace GObject.Introspection.CodeGen.Syntax
{

    class PropertyOrFieldExpressionBuilder : SyntaxExpressionBuilderBase<PropertyOrFieldExpression>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="expression"></param>
        public PropertyOrFieldExpressionBuilder(ModuleContext context, PropertyOrFieldExpression expression) :
            base(context, expression)
        {

        }

        public override SyntaxNode Build()
        {
            return Syntax.MemberAccessExpression(Context.Build(Expression.Instance), Expression.MemberName);
        }

    }

}