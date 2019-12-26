using System;

namespace GObject.Introspection.CodeGen.Model.Expressions
{

    /// <summary>
    /// Represents a conversion from one type to another.
    /// </summary>
    class ConvertExpression : Expression
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        public ConvertExpression(Context context, ITypeSymbol type, Expression value) :
            base(context, type)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Value to be converted.
        /// </summary>
        public Expression Value { get; }

    }

}
