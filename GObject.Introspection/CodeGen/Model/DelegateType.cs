using System;
using System.Collections.Generic;
using System.Linq;

namespace GObject.Introspection.CodeGen.Model
{

    /// <summary>
    /// Describes a type that is a delegate.
    /// </summary>
    abstract class DelegateType : Type
    {

        readonly Lazy<IReadOnlyList<Parameter>> parameters;
        readonly Lazy<ITypeSymbol> returnType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        internal DelegateType(Context context) :
            base(context)
        {
            parameters = new Lazy<IReadOnlyList<Parameter>>(() => GetParameters().ToList());
            returnType = new Lazy<ITypeSymbol>(GetReturnType);
        }

        /// <summary>
        /// Gets the arguments that describe the delegate.
        /// </summary>
        public IReadOnlyList<Parameter> Parameters => parameters.Value;

        /// <summary>
        /// Gets the arguments that describe the delegate.
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerable<Parameter> GetParameters();

        /// <summary>
        /// Gets the return type of the method.
        /// </summary>
        public ITypeSymbol ReturnType => returnType.Value;

        /// <summary>
        /// Gets the return type of the delegate.
        /// </summary>
        /// <returns></returns>
        protected abstract ITypeSymbol GetReturnType();

    }

}
