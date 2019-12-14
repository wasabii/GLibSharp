using System;

using GObject.Introspection.Model;

namespace GObject.Introspection.Reflection
{

    class ConstructorElementMember : MethodMember
    {

        readonly Constructor constructor;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        /// <param name="constructor"></param>
        public ConstructorElementMember(IntrospectionContext context, IntrospectionType declaringType, Constructor constructor) :
            base(context, declaringType)
        {
            this.constructor = constructor ?? throw new ArgumentNullException(nameof(constructor));
        }

        /// <summary>
        /// Gets the name of the member.
        /// </summary>
        public override string Name => null;

        /// <summary>
        /// Gets the return type of the method.
        /// </summary>
        /// <returns></returns>
        protected override TypeSymbol GetReturnType()
        {
            return null;
        }

        public override IntrospectionInvokable GetInvokable()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return Modifiers.HasFlag(IntrospectionMemberModifier.Static) ? "cctor" : "ctor";
        }

    }

}
