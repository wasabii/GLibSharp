using System;

using GObject.Introspection.CodeGen.Model.Expressions;

using Microsoft.CodeAnalysis;

namespace GObject.Introspection.CodeGen.Syntax
{

    class BinaryExpressionBuilder : SyntaxExpressionBuilderBase<BinaryExpression>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="expression"></param>
        public BinaryExpressionBuilder(ModuleContext context, BinaryExpression expression) :
            base(context, expression)
        {

        }

        public override SyntaxNode Build()
        {
            var l = Context.Build(Expression.Left);
            var r = Context.Build(Expression.Right);

            switch (Expression.NodeType)
            {
                case BinaryExpressionType.Equal:
                    return Syntax.ValueEqualsExpression(l, r);
                case BinaryExpressionType.GreaterThan:
                    return Syntax.GreaterThanExpression(l, r);
                case BinaryExpressionType.LessThan:
                    return Syntax.LessThanExpression(l, r);
                case BinaryExpressionType.GreaterThanOrEqual:
                    return Syntax.GreaterThanOrEqualExpression(l, r);
                case BinaryExpressionType.LessThanOrEqual:
                    return Syntax.LessThanOrEqualExpression(l, r);
                case BinaryExpressionType.NotEqual:
                    return Syntax.ValueNotEqualsExpression(l, r);
                case BinaryExpressionType.AndAlso:
                    return Syntax.LogicalAndExpression(l, r);
                case BinaryExpressionType.OrElse:
                    return Syntax.LogicalOrExpression(l, r);
                case BinaryExpressionType.ExclusiveOr:
                    return Syntax.ExclusiveOrExpression(l, r);
                default:
                    throw new InvalidOperationException();
            }
        }

    }

}