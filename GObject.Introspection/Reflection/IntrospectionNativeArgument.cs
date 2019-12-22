using System;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes an argument on the native extern method.
    /// </summary>
    public class IntrospectionNativeArgument
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        public IntrospectionNativeArgument(string name, TypeSymbol type)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Type = type ?? throw new ArgumentNullException(nameof(type));
        }

        /// <summary>
        /// Gets the name of the native argument.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the type of the native argument.
        /// </summary>
        public TypeSymbol Type { get; }

    }

}
