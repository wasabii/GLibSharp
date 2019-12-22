using System;

namespace GObject.Introspection.Model
{

    public abstract class MethodMember : Member
    {

        readonly Lazy<TypeSpec> returnType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        internal MethodMember(Context context, Type declaringType) :
            base(context, declaringType)
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
