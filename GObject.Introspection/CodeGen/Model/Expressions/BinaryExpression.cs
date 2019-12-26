using System;
using System.Collections.Generic;
using System.Linq;

namespace GObject.Introspection.CodeGen.Model.Expressions
{

    class BinaryExpression : Expression
    {

        /// <summary>
        /// Returns an expression which is the AndAlso of the given expressions.
        /// </summary>
        /// <param name="expressions"></param>
        /// <returns></returns>
        public static Expression AndAlso(IEnumerable<Expression> expressions)
        {
            Expression e = null;

            foreach (var i in expressions)
                e = e == null ? i : new BinaryExpression(i.Context, BinaryExpressionType.AndAlso, e, i);

            return e;
        }

        /// <summary>
        /// Returns an expression which is the AndAlso of the given expressions.
        /// </summary>
        /// <param name="expressions"></param>
        /// <returns></returns>
        public static Expression AndAlso(params Expression[] expressions)
        {
            return AndAlso(expressions.AsEnumerable());
        }

        /// <summary>
        /// Returns an expression which is the OrElse of the given expressions.
        /// </summary>
        /// <param name="expressions"></param>
        /// <returns></returns>
        public static Expression OrElse(IEnumerable<Expression> expressions)
        {
            Expression e = null;

            foreach (var i in expressions)
                e = e == null ? i : new BinaryExpression(i.Context, BinaryExpressionType.OrElse, e, i);

            return e;
        }

        /// <summary>
        /// Returns an expression which is the OrElse of the given expressions.
        /// </summary>
        /// <param name="expressions"></param>
        /// <returns></returns>
        public static Expression OrElse(params Expression[] expressions)
        {
            return OrElse(expressions.AsEnumerable());
        }

        /// <summary>
        /// Gets the return type for the given expression type.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        static ITypeSymbol GetExpressionType(Context context, BinaryExpressionType type, Expression left, Expression right)
        {
            switch (type)
            {
                case BinaryExpressionType.AndAlso:
                case BinaryExpressionType.OrElse:
                case BinaryExpressionType.Equal:
                case BinaryExpressionType.NotEqual:
                case BinaryExpressionType.GreaterThan:
                case BinaryExpressionType.GreaterThanOrEqual:
                case BinaryExpressionType.LessThan:
                case BinaryExpressionType.LessThanOrEqual:
                    return context.ResolveManagedSymbol(typeof(bool).FullName);
                case BinaryExpressionType.ExclusiveOr:
                    return left.Type;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public BinaryExpression(Context context, BinaryExpressionType type, Expression left, Expression right) :
            base(context, GetExpressionType(context, type, left, right))
        {
            NodeType = type;
            Left = left ?? throw new ArgumentNullException(nameof(left));
            Right = right ?? throw new ArgumentNullException(nameof(right));
        }

        /// <summary>
        /// Gets the type of binary expression.
        /// </summary>
        public BinaryExpressionType NodeType { get; }

        /// <summary>
        /// Gets the expression which appears on the left.
        /// </summary>
        public Expression Left { get; }

        /// <summary>
        /// Gets the expression which appears on the right.
        /// </summary>
        public Expression Right { get; }

    }

}
