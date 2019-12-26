using System;

namespace GObject.Introspection.CodeGen.Model
{

    /// <summary>
    /// Describes a evaluable that results in a value.
    /// </summary>
    abstract class Expression : Evaluable
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        public Expression(Context context, ITypeSymbol type) :
            base(context)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
        }

        /// <summary>
        /// Gets the type of the value of the expression.
        /// </summary>
        public ITypeSymbol Type { get; }

    }

}
