using System;

namespace GObject.Introspection.CodeGen.Model.Expressions
{

    class ConditionalExpression : Expression
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="if"></param>
        /// <param name="then"></param>
        /// <param name="else"></param>
        public ConditionalExpression(Context context, Expression @if, Expression @then, Expression @else) :
            base(context, context.ResolveManagedSymbol(typeof(bool).FullName))
        {
            If = @if ?? throw new ArgumentNullException(nameof(@if));
            Then = then ?? throw new ArgumentNullException(nameof(then));
            Else = @else ?? throw new ArgumentNullException(nameof(@else));
        }

        /// <summary>
        /// If the given expression returns <c>true</c>.
        /// </summary>
        public Expression If { get; }

        /// <summary>
        /// Then return the value of the specified expression.
        /// </summary>
        public Expression Then { get; }

        /// <summary>
        /// Else return the value of this expression.
        /// </summary>
        public Expression Else { get; }

    }

}
