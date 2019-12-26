using System;

namespace GObject.Introspection.CodeGen.Model.Expressions
{

    class IsTypeExpression : Expression
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="isType"></param>
        public IsTypeExpression(Context context, Expression expression, ITypeSymbol isType) :
            base(context, context.ResolveManagedSymbol(typeof(bool).FullName))
        {
            Expression = expression ?? throw new ArgumentNullException(nameof(expression));
            IsType = isType ?? throw new ArgumentNullException(nameof(isType));
        }

        /// <summary>
        /// Expression to check.
        /// </summary>
        public Expression Expression { get; }

        /// <summary>
        /// Type to check.
        /// </summary>
        public ITypeSymbol IsType { get; }

    }

}
