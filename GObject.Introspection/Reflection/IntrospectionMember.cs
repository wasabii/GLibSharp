using System;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes a member of an introspection type.
    /// </summary>
    public abstract class IntrospectionMember
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        public IntrospectionMember(IntrospectionContext context, IntrospectionType declaringType)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            DeclaringType = declaringType ?? throw new ArgumentNullException(nameof(declaringType));
        }

        /// <summary>
        /// Gets the current introspection context of the type.
        /// </summary>
        protected IntrospectionContext Context { get; }

        /// <summary>
        /// Gets the parent type of this member.
        /// </summary>
        public IntrospectionType DeclaringType { get; }

        /// <summary>
        /// Gets the name of the member.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the kind of the member.
        /// </summary>
        public abstract IntrospectionMemberKind Kind { get; }

        /// <summary>
        /// Gets the visibility of the member.
        /// </summary>
        public virtual IntrospectionVisibility Visibility => IntrospectionVisibility.Public;

        /// <summary>
        /// Gets the modifiers applied to the member.
        /// </summary>
        public virtual IntrospectionMemberModifier Modifiers => IntrospectionMemberModifier.Default;

    }

}
