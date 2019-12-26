using System.Collections.Generic;

namespace GObject.Introspection.CodeGen.Model
{

    /// <summary>
    /// Describes a reference to either an introspected type or a system type.
    /// </summary>
    interface ITypeSymbol
    {

        /// <summary>
        /// Gets the qualified CLR type name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Returns <c>true</c> if the type symbol references an array.
        /// </summary>
        bool IsArray { get; }

        /// <summary>
        /// Returns <c>true</c> if the type is blittable to the native version of the type.
        /// </summary>
        bool IsBlittable { get; }

        /// <summary>
        /// Returns <c>true</c> if the type is a generic type.
        /// </summary>
        bool IsGenericType { get; }

        /// <summary>
        /// Returns <c>true</c> if the type is a generic type that must be closed.
        /// </summary>
        bool IsOpenGenericType { get; }

        /// <summary>
        /// Gets the type parameters.
        /// </summary>
        IReadOnlyList<ITypeParameterSymbol> TypeParameters { get; }

        /// <summary>
        /// Gets the type arguments.
        /// </summary>
        IReadOnlyList<ITypeSymbol> TypeArguments { get; }

        /// <summary>
        /// Makes a new <see cref="ITypeSymbol"/> by filling in the specified type arguments.
        /// </summary>
        /// <param name="typeArguments"></param>
        /// <returns></returns>
        ITypeSymbol MakeGenericType(params ITypeSymbol[] typeArguments);

    }

}
