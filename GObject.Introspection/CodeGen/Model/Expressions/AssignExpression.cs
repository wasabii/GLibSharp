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
        /// <param name="target"></param>
        /// <param name="member"></param>
        /// <param name="value"></param>
        public AssignExpression(Context context, Expression target, string member, Expression value) :
            base(context, target.Type)
        {
            Target = target ?? throw new ArgumentNullException(nameof(target));
            Member = member;
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="target"></param>
        /// <param name="value"></param>
        public AssignExpression(Context context, Expression target, Expression value) :
            this(context, target, null, value)
        {

        }

        /// <summary>
        /// Gets the variable to be set by the expression.
        /// </summary>
        public Expression Target { get; }

        /// <summary>
        /// Name of the property or field of the variable to set.
        /// </summary>
        public string Member { get; }

        /// <summary>
        /// Gets the value to be set by the expression.
        /// </summary>
        public Expression Value { get; }

    }

}
