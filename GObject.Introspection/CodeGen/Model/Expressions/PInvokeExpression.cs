using System;
using System.Collections.Generic;

namespace GObject.Introspection.CodeGen.Model.Expressions
{

    /// <summary>
    /// Describes the invokation of a native function.
    /// </summary>
    class PInvokeExpression : Expression
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="function"></param>
        /// <param name="parameters"></param>
        public PInvokeExpression(Context context, NativeFunction function, IReadOnlyList<Expression> parameters) :
            base(context, function.ReturnArgument?.Type)
        {
            Function = function ?? throw new ArgumentNullException(nameof(function));
            Parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
        }

        /// <summary>
        /// Gets the native function to be invoked.
        /// </summary>
        public NativeFunction Function { get; }

        /// <summary>
        /// Gets the values to be passed to the invoked function.
        /// </summary>
        public IReadOnlyList<Expression> Parameters { get; }

    }

}
