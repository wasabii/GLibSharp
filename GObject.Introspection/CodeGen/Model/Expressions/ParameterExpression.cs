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
        /// <param name="parameter"></param>
        internal ParameterExpression(Context context, Parameter parameter) :
            base(context, parameter.Name, parameter.Type)
        {
            Parameter = parameter ?? throw new ArgumentNullException(nameof(parameter));
        }

        /// <summary>
        /// Gets the argument corresponding with the parameter.
        /// </summary>
        public Parameter Parameter { get; }

    }

}
