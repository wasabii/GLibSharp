using System;

namespace GObject.Introspection.Reflection
{

    public abstract class MethodMember : IntrospectionMember
    {

        readonly Lazy<TypeSymbol> returnType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public MethodMember(IntrospectionContext context) :
            base(context)
        {
            returnType = new Lazy<TypeSymbol>(GetReturnType);
        }

        /// <summary>
        /// Gets the kind of the member.
        /// </summary>
        public override IntrospectionMemberKind Kind => IntrospectionMemberKind.Method;

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
