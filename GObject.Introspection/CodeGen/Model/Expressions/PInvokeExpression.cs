﻿using System;
using System.Collections.Generic;
using System.Linq;

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
        /// <param name="arguments"></param>
        public PInvokeExpression(Context context, NativeFunction function, IReadOnlyList<Expression> arguments) :
            base(context, function.ReturnType)
        {
            Function = function ?? throw new ArgumentNullException(nameof(function));
            Arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="function"></param>
        /// <param name="arguments"></param>
        public PInvokeExpression(Context context, NativeFunction function, params Expression[] arguments) :
            this(context, function, arguments?.ToList())
        {

        }

        /// <summary>
        /// Gets the native function to be invoked.
        /// </summary>
        public NativeFunction Function { get; }

        /// <summary>
        /// Gets the values to be passed to the invoked function.
        /// </summary>
        public IReadOnlyList<Expression> Arguments { get; }

    }

}
