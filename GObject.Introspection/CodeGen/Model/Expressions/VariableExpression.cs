using System;

namespace GObject.Introspection.CodeGen.Model.Expressions
{

    /// <summary>
    /// Describes an expression that stores a value.
    /// </summary>
    class VariableExpression : Expression
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        public VariableExpression(Context context, string name, ITypeSymbol type) :
            base(context, type)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        /// <summary>
        /// Gets the name of the variable.
        /// </summary>
        public string Name { get; }

    }

}
