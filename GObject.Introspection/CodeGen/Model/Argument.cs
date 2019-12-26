using System;

namespace GObject.Introspection.CodeGen.Model
{

    /// <summary>
    /// Describes an argument relating to an invokable.
    /// </summary>
    class Argument
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="modifier"></param>
        public Argument(string name, ITypeSymbol type, ArgumentModifier modifier = ArgumentModifier.In)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Modifier = modifier;
        }

        /// <summary>
        /// Gets the name of the variable.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the type of the variable.
        /// </summary>
        public ITypeSymbol Type { get; }

        /// <summary>
        /// Describes the direction of the argument.
        /// </summary>
        public ArgumentModifier Modifier { get; }

    }

}
