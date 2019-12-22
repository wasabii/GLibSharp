using System;
using System.Text;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes a native type symbol that is further qualified.
    /// </summary>
    public class NativeQualifiedTypeSymbol : NativeTypeSymbol
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public NativeQualifiedTypeSymbol(NativeTypeSymbol type, NativeTypeSymbolQualifier qualifier)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Qualifier = qualifier;
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
            var b = new StringBuilder();
            if (Qualifier.HasFlag(NativeTypeSymbolQualifier.Const))
                b.Append("const ");
            if (Qualifier.HasFlag(NativeTypeSymbolQualifier.Volatile))
                b.Append("volatile ");

            b.Append(Type.Name);
            return b.ToString();
        }

        public NativeTypeSymbolQualifier Qualifier { get; }

        /// <summary>
        /// Gets the qualified type.
        /// </summary>
        public NativeTypeSymbol Type { get; }

    }

}
