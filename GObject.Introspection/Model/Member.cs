using System;

namespace GObject.Introspection.Model
{

    /// <summary>
    /// Describes a member of an introspection type.
    /// </summary>
    public abstract class Member
    {

        readonly Context context;
        readonly Type declaringType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        internal Member(Context context, Type declaringType)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.declaringType = declaringType ?? throw new ArgumentNullException(nameof(declaringType));
        }

        /// <summary>
        /// Gets the current introspection context of the type.
        /// </summary>
        internal Context Context => context;

        /// <summary>
        /// Gets the parent type of this member.
        /// </summary>
        public Type DeclaringType => declaringType;

        /// <summary>
        /// Gets the name of the member.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the visibility of the member.
        /// </summary>
        public virtual Visibility Visibility => Visibility.Public;

        /// <summary>
        /// Gets the modifiers applied to the member.
        /// </summary>
        public virtual MemberModifier Modifiers => MemberModifier.Default;

    }

}
