using System;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes a native type symbol that represents a pointer to another native type.
    /// </summary>
    public class NativePointerTypeSymbol : NativeTypeSymbol
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public NativePointerTypeSymbol(NativeTypeSymbol type)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
        }

        /// <summary>
        /// Gets the name of the native type.
        /// </summary>
        public override string Name => GetName();

        /// <summary>
        /// Returns a string representation of this symbol.
        /// </summary>
        /// <returns></returns>
        string GetName()
        {
            return Type.Name + "*";
        }

        /// <summary>
        /// Gets the pointed to type.
        /// </summary>
        public NativeTypeSymbol Type { get; }

    }

}
