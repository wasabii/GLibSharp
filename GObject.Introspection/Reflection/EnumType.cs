using System.Collections.Generic;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes an enum type.
    /// </summary>
    public abstract class EnumType : IntrospectionType
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        internal EnumType(IntrospectionContext context) :
            base(context)
        {

        }

        protected sealed override IEnumerable<IntrospectionMember> GetMembers()
        {
            return GetMemberMembers();
        }

        /// <summary>
        /// Gets the members of the enum.
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerable<EnumMember> GetMemberMembers();

    }

}
