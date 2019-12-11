namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes the kinds of <see cref="IntrospectionType"/>s.
    /// </summary>
    public enum IntrospectionTypeKind
    {

        /// <summary>
        /// Type is a .NET class.
        /// </summary>
        Class,

        /// <summary>
        /// Type is a .NET struct.
        /// </summary>
        Struct,

        /// <summary>
        /// Type is a .NET enum.
        /// </summary>
        Enum,

        /// <summary>
        /// Type is a .NET delegate.
        /// </summary>
        Delegate,

        /// <summary>
        /// Type is a .NET interface.
        /// </summary>
        Interface,

    }

}
