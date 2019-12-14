namespace GObject.Introspection.Reflection
{

    public abstract class EnumerationMember : IntrospectionMember
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        public EnumerationMember(IntrospectionContext context, IntrospectionType declaringType) :
            base(context, declaringType)
        {

        }

        /// <summary>
        /// Gets the kind of the member.
        /// </summary>
        public sealed override IntrospectionMemberKind Kind => IntrospectionMemberKind.Member;

        /// <summary>
        /// Gets the value of the enumeration member.
        /// </summary>
        public abstract int Value { get; }

    }

}
