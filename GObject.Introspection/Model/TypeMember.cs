using System;

namespace GObject.Introspection.Model
{

    /// <summary>
    /// Describes a member of a type which is itself a type.
    /// </summary>
    public class TypeMember : Member
    {

        readonly Type type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        /// <param name="type"></param>
        internal TypeMember(Context context, Type declaringType, Type type) :
            base(context, declaringType)
        {
            this.type = type ?? throw new ArgumentNullException(nameof(type));
        }

        /// <summary>
        /// Gets the name of the member.
        /// </summary>
        public override string Name => type.Name;

        /// <summary>
        /// Gets the type that is a member.
        /// </summary>
        public Type Type => type;

    }

}
