using System;

namespace GObject.Introspection.Model
{

    /// <summary>
    /// Describes a type symbol refering to an existing CLR type.
    /// </summary>
    class ManagedTypeSymbol : TypeSymbol
    {

        /// <summary>
        /// Derives a <see cref="ManagedTypeSymbol"/> from the specified managed type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ManagedTypeSymbol FromType(IManagedTypeReference type)
        {
            return new ManagedTypeSymbol(type);
        }

        readonly IManagedTypeReference reference;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="reference"></param>
        public ManagedTypeSymbol(IManagedTypeReference reference)
        {
            this.reference = reference ?? throw new ArgumentNullException(nameof(reference));
        }

        /// <summary>
        /// Gets the qualified CLR type name.
        /// </summary>
        public override string Name => reference.Name;

        /// <summary>
        /// Returns whether or not the referenced type is an array.
        /// </summary>
        public override bool IsArray => reference.IsArray;

        /// <summary>
        /// Gets whether or not the type is blittable.
        /// </summary>
        public override bool IsBlittable => reference.IsBlittable;

    }

}
