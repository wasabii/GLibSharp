using System;
using System.Reflection;
using System.Reflection.Emit;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes a type that is a delegate.
    /// </summary>
    public abstract class DelegateType : IntrospectionType
    {

        readonly Lazy<TypeSymbol> returnType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public DelegateType(IntrospectionContext context) :
            base(context)
        {
            returnType = new Lazy<TypeSymbol>(GetReturnType);
        }

        public sealed override IntrospectionTypeKind Kind => IntrospectionTypeKind.Delegate;

        /// <summary>
        /// Gets the return type of the method.
        /// </summary>
        public TypeSymbol ReturnType => returnType.Value;

        /// <summary>
        /// Gets the method type.
        /// </summary>
        /// <returns></returns>
        protected virtual TypeSymbol GetReturnType() => GetInvokable().Return?.Type;

        public abstract IntrospectionInvokable GetInvokable();

    }

}
