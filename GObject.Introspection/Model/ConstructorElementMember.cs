using System;

using GObject.Introspection.Xml;

namespace GObject.Introspection.Model
{

    class ConstructorElementMember : ConstructorMember
    {

        readonly ConstructorElement constructor;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        /// <param name="constructor"></param>
        internal ConstructorElementMember(Context context, Type declaringType, ConstructorElement constructor) :
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
            return Modifiers.HasFlag(MemberModifier.Static) ? "cctor" : "ctor";
        }

    }

}
