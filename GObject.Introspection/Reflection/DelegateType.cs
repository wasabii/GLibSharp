using System;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes a type that is a delegate.
    /// </summary>
    public abstract class DelegateType : IntrospectionType
    {

        readonly Lazy<TypeSpec> returnType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        internal DelegateType(IntrospectionContext context) :
            base(context)
        {
            returnType = new Lazy<TypeSpec>(GetReturnType);
        }

        /// <summary>
        /// Gets the return type of the method.
        /// </summary>
        public TypeSpec ReturnType => returnType.Value;

        /// <summary>
        /// Gets the method type.
        /// </summary>
        /// <returns></returns>
        protected virtual TypeSpec GetReturnType() => GetInvokable().Return?.Type;

        public abstract IntrospectionInvokable GetInvokable();

    }

}
