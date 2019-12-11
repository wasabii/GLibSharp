using System;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes a member of a type which is itself a type.
    /// </summary>
    public class IntrospectionTypeMember : IntrospectionMember
    {

        readonly IntrospectionType type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        public IntrospectionTypeMember(IntrospectionContext context, IntrospectionType type) :
            base(context)
        {
            this.type = type ?? throw new ArgumentNullException(nameof(type));
        }

        /// <summary>
        /// Gets the name of the member.
        /// </summary>
        public override string Name => type.Name;

        /// <summary>
        /// Gets the kind of the member.
        /// </summary>
        public override IntrospectionMemberKind Kind => IntrospectionMemberKind.Type;

    }

}
