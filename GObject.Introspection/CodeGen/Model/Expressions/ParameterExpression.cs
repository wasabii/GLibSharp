using System;

namespace GObject.Introspection.CodeGen.Model.Expressions
{

    /// <summary>
    /// Describes a value accessed from the enclosing invokables parameters.
    /// </summary>
    class ParameterExpression : VariableExpression
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        public ParameterExpression(Context context, Argument argument) :
            base(context, argument.Name, argument.Type)
        {
            Argument = argument ?? throw new ArgumentNullException(nameof(argument));
        }

        /// <summary>
        /// Gets the argument corresponding with the parameter.
        /// </summary>
        public Argument Argument { get; }

    }

}
