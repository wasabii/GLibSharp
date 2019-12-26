using System.Collections.Generic;

namespace GObject.Introspection.CodeGen.Model
{

    /// <summary>
    /// Describes an enum type.
    /// </summary>
    abstract class EnumType : Type
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        internal EnumType(Context context) :
            base(context)
        {

        }

        protected sealed override IEnumerable<Member> GetMembers()
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
