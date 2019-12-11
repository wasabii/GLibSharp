namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes an argument relating to an invokable.
    /// </summary>
    public class IntrospectionArgument
    {

        /// <summary>
        /// Gets the name of the introspected argument.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the type of the argument.
        /// </summary>
        public TypeSymbol Type { get; }

        /// <summary>
        /// Describes the direction of the argument.
        /// </summary>
        public IntrospectionArgumentDirection Direction { get; }

    }

}
