using System;

namespace GObject.Introspection.CodeGen.Model.Expressions
{

    class ReturnStatement : Statement
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="expression"></param>
        public ReturnStatement(Context context, Expression expression) :
            base(context)
        {
            Expression = expression ?? throw new ArgumentNullException(nameof(expression));
        }

        /// <summary>
        /// Expression to return.
        /// </summary>
        public Expression Expression { get; }

    }

}
