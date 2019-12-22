using System;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes a member of an introspection type.
    /// </summary>
    public abstract class IntrospectionMember
    {

        readonly IntrospectionContext context;
        readonly IntrospectionType declaringType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        internal IntrospectionMember(IntrospectionContext context, IntrospectionType declaringType)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.declaringType = declaringType ?? throw new ArgumentNullException(nameof(declaringType));
        }

        /// <summary>
        /// Gets the current introspection context of the type.
        /// </summary>
        internal IntrospectionContext Context => context;

        /// <summary>
        /// Gets the parent type of this member.
        /// </summary>
        public IntrospectionType DeclaringType => declaringType;

        /// <summary>
        /// Gets the name of the member.
        /// </summary>
        public abstract string Name { get; }

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
