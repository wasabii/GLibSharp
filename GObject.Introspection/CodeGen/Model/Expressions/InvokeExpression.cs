using System;
using System.Collections.Generic;
using System.Linq;

namespace GObject.Introspection.CodeGen.Model.Expressions
{

    /// <summary>
    /// Describes the invokation of a method.
    /// </summary>
    class InvokeExpression : Expression
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="methodType"></param>
        /// <param name="methodName"></param>
        /// <param name="returnType"></param>
        /// <param name="instance"></param>
        /// <param name="parameters"></param>
        public InvokeExpression(Context context, ITypeSymbol methodType, string methodName, ITypeSymbol returnType, Expression instance, IReadOnlyList<Expression> parameters) :
            this(context, methodType, methodName, returnType, parameters)
        {
            Instance = instance ?? throw new ArgumentNullException(nameof(instance));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="methodType"></param>
        /// <param name="methodName"></param>
        /// <param name="returnType"></param>
        /// <param name="instance"></param>
        /// <param name="parameters"></param>
        public InvokeExpression(Context context, ITypeSymbol methodType, string methodName, ITypeSymbol returnType, Expression instance, params Expression[] parameters) :
            this(context, methodType, methodName, returnType, instance, parameters.ToList())
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="methodType"></param>
        /// <param name="methodName"></param>
        /// <param name="returnType"></param>
        /// <param name="parameters"></param>
        public InvokeExpression(Context context, ITypeSymbol methodType, string methodName, ITypeSymbol returnType, IReadOnlyList<Expression> parameters) :
            base(context, returnType)
        {
            MethodType = methodType ?? throw new ArgumentNullException(nameof(methodType));
            MethodName = methodName ?? throw new ArgumentNullException(nameof(methodName));
            Parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="methodType"></param>
        /// <param name="methodName"></param>
        /// <param name="returnType"></param>
        /// <param name="parameters"></param>
        public InvokeExpression(Context context, ITypeSymbol methodType, string methodName, ITypeSymbol returnType, params Expression[] parameters) :
            this(context, methodType, methodName, returnType, parameters.ToList())
        {

        }

        /// <summary>
        /// Gets the method to be invoked.
        /// </summary>
        public ITypeSymbol MethodType { get; }

        /// <summary>
        /// Name of the method to be invoked.
        /// </summary>
        public string MethodName { get; }

        /// <summary>
        /// Instance upon which to invoke the method.
        /// </summary>
        public Expression Instance { get; }

        /// <summary>
        /// Gets the values to be passed to the invoked function.
        /// </summary>
        public IReadOnlyList<Expression> Parameters { get; }

    }

}
