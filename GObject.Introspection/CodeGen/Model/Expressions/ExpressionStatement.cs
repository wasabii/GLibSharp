using System;

namespace GObject.Introspection.CodeGen.Model.Expressions
{

    /// <summary>
    /// Describes a statement which is an expression where the return value is discarded.
    /// </summary>
    class ExpressionStatement : Statement
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="expression"></param>
        public ExpressionStatement(Context context, Expression expression) :
            base(context)
        {
            Expression = expression ?? throw new ArgumentNullException(nameof(expression));
        }

        /// <summary>
        /// Gets the expresson to execute.
        /// </summary>
        public Expression Expression { get; }

    }

}
