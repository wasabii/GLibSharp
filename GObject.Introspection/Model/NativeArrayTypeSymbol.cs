using System;

namespace GObject.Introspection.Model
{

    /// <summary>
    /// Describes a native type symbol that represents a pointer to another native type.
    /// </summary>
    public class NativeArrayTypeSymbol : NativeTypeSymbol
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public NativeArrayTypeSymbol(NativeTypeSymbol type, int? size = null)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Size = size;
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
            return Type.Name + "[]";
        }

        /// <summary>
        /// Gets the pointed to type.
        /// </summary>
        public NativeTypeSymbol Type { get; }

        /// <summary>
        /// Optionally gets the size of the array.
        /// </summary>
        public int? Size { get; }

    }

}
