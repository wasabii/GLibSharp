using System.Collections.Generic;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes a flag type generated from a bitfield.
    /// </summary>
    public abstract class EnumType : IntrospectionType
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="flag"></param>
        public EnumType(IntrospectionContext context) :
            base(context)
        {

        }

        public sealed override IntrospectionTypeKind Kind => IntrospectionTypeKind.Enum;

        protected sealed override IEnumerable<IntrospectionMember> GetMembers()
        {
            return GetMemberMembers();
        }

        /// <summary>
        /// Gets the members of the enum.
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerable<EnumerationMember> GetMemberMembers();

    }

}
