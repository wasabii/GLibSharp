namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes a member of an enumeration.
    /// </summary>
    public abstract class EnumMember : IntrospectionMember
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        internal EnumMember(IntrospectionContext context, IntrospectionType declaringType) :
            base(context, declaringType)
        {

        }

        /// <summary>
        /// Gets the value of the enumeration member.
        /// </summary>
        public abstract int Value { get; }

    }

}
