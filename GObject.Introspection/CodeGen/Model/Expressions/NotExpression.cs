using System;

namespace GObject.Introspection.CodeGen.Model.Expressions
{

    class NotExpression : Expression
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="expression"></param>
        public NotExpression(Context context, Expression expression) :
            base(context, expression.Type)
        {
            Expression = expression ?? throw new ArgumentNullException(nameof(expression));
        }

        /// <summary>
        /// Gets the name of the variable.
        /// </summary>
        public Expression Expression { get; }

    }

}
