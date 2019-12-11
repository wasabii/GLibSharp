using System;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes an array of another type.
    /// </summary>
    class ArrayTypeSymbol : TypeSymbol
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="baseType"></param>
        public ArrayTypeSymbol(TypeSymbol baseType)
        {
            BaseType = baseType ?? throw new ArgumentNullException(nameof(baseType));
        }

        /// <summary>
        /// Returns <c>true</c>.
        /// </summary>
        public override bool IsArray => true;

        /// <summary>
        /// Gets the type that is represented as an array.
        /// </summary>
        public TypeSymbol BaseType { get; }

        /// <summary>
        /// Gets the qualified name of the type symbol.
        /// </summary>
        public override string QualifiedName => BaseType.QualifiedName + "[]";

    }

}
