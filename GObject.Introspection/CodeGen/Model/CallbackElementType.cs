using System;
using System.Collections.Generic;
using System.Linq;

using GObject.Introspection.Library.Model;

namespace GObject.Introspection.CodeGen.Model
{

    /// <summary>
    /// Describes a delegate generated from a introspected callback.
    /// </summary>
    class CallbackElementType : DelegateType
    {

        readonly CallbackElement callback;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="callback"></param>
        public CallbackElementType(Context context, CallbackElement callback) :
            base(context)
        {
            this.callback = callback ?? throw new ArgumentNullException(nameof(callback));
        }

        public override string Name => callback.Name;

        /// <summary>
        /// Gets the original introspected name of the type.
        /// </summary>
        public override string IntrospectionName => callback.Name;

        public override string NativeName => callback.CType;

        /// <summary>
        /// Gets the arguments that describe the delegate.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<Parameter> GetParameters()
        {
            return callback.Parameters.Select(i => GetArgument(i));
        }

        /// <summary>
        /// Gets the argument for the parameter.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        Parameter GetArgument(IParameter parameter)
        {
            switch (parameter)
            {
                case ParameterElement p:
                    return GetArgument(p);
                default:
                    throw new InvalidOperationException("Invalid parameter type.");
            }
        }

        /// <summary>
        /// Gets the argument for the parameter.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        Parameter GetArgument(ParameterElement parameter)
        {
            var typeInfo = parameter.Type;
            if (typeInfo == null)
                throw new InvalidOperationException("No type specified for parameter.");

            var typeSpec = typeInfo.ToSpec(Context);
            if (typeSpec == null)
                throw new InvalidOperationException("Could not resolve type spec for parameter.");

            return new Parameter(Context, parameter.Name, typeSpec.Type);
        }

        /// <summary>
        /// Gets the return argument of the delegate.
        /// </summary>
        /// <returns></returns>
        protected override ITypeSymbol GetReturnType()
        {
            var returnTypeInfo = callback.ReturnValue?.Type;
            if (returnTypeInfo == null)
                return null;

            var returnSpec = returnTypeInfo.ToSpec(Context);
            if (returnSpec == null)
                throw new InvalidOperationException("Could not resolve type specification for callback return.");

            return returnSpec.Type;
        }

    }

}
