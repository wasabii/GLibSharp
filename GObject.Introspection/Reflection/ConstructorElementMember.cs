using System;

using GObject.Introspection.Model;

namespace GObject.Introspection.Reflection
{

    class ConstructorElementMember : ConstructorMember
    {

        readonly Constructor constructor;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        /// <param name="constructor"></param>
        internal ConstructorElementMember(IntrospectionContext context, IntrospectionType declaringType, Constructor constructor) :
            base(context, declaringType)
        {
            this.constructor = constructor ?? throw new ArgumentNullException(nameof(constructor));
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
