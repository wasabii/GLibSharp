using System;

namespace GObject.Introspection.Model
{

    /// <summary>
    /// Describes an argument relating to an invokable.
    /// </summary>
    public class Argument
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="direction"></param>
        public Argument(string name, TypeSpec type, ArgumentDirection direction = ArgumentDirection.In)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Direction = direction;
        }

        /// <summary>
        /// Gets the name of the introspected argument.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the type of the argument.
        /// </summary>
        public TypeSpec Type { get; }

        /// <summary>
        /// Describes the direction of the argument.
        /// </summary>
        public ArgumentDirection Direction { get; }

    }

}
