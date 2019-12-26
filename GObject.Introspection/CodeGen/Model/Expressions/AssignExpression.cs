using System;

namespace GObject.Introspection.CodeGen.Model.Expressions
{

    /// <summary>
    /// Describes an expression that assigns a value to a variable.
    /// </summary>
    class AssignExpression : Expression
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="variable"></param>
        /// <param name="value"></param>
        public AssignExpression(Context context, VariableExpression variable, Expression value) :
            base(context, variable.Type)
        {
            Variable = variable ?? throw new ArgumentNullException(nameof(variable));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets the variable to be set by the expression.
        /// </summary>
        public VariableExpression Variable { get; }

        /// <summary>
        /// Gets the value to be set by the expression.
        /// </summary>
        public Expression Value { get; }

    }

}
