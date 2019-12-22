using System;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes an introspection type definition.
    /// </summary>
    class IntrospectionTypeDef
    {

        /// <summary>
        /// Converts the type def to the underlying type.
        /// </summary>
        /// <param name="def"></param>
        public static implicit operator IntrospectionType(IntrospectionTypeDef def)
        {
            return def?.Type;
        }

        readonly Lazy<IntrospectionType> type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="introspectedName"></param>
        /// <param name="name"></param>
        /// <param name="nativeName"></param>
        /// <param name="getType"></param>
        public IntrospectionTypeDef(string name, string introspectedName, string nativeName, Func<IntrospectionType> getType) :
            this(name, getType)
        {
            IntrospectionName = introspectedName;
            NativeName = nativeName;

            type = new Lazy<IntrospectionType>(getType ?? throw new ArgumentNullException(nameof(getType)));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="getType"></param>
        public IntrospectionTypeDef(string name, Func<IntrospectionType> getType)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));

            type = new Lazy<IntrospectionType>(getType ?? throw new ArgumentNullException(nameof(getType)));
        }

        /// <summary>
        /// Gets the name of the managed type represented by this introspected type.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the name of the original repository type, if available.
        /// </summary>
        public string IntrospectionName { get; }

        /// <summary>
        /// Gets the name of the native type represented by this introspected type.
        /// </summary>
        public string NativeName { get; }

        /// <summary>
        /// Gets the introspected type information.
        /// </summary>
        public IntrospectionType Type => type.Value;

    }

}
