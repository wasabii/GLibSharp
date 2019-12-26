using System;

using GObject.Introspection.Library.Model;

namespace GObject.Introspection.CodeGen.Model
{

    /// <summary>
    /// Describes a type symbol refering to a primitive.
    /// </summary>
    class PrimitiveNativeTypeSymbol : NativeTypeSymbol
    {

        readonly IHasCType element;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public PrimitiveNativeTypeSymbol(IHasCType element)
        {
            this.element = element ?? throw new ArgumentNullException(nameof(element));
        }

        /// <summary>
        /// Gets the qualified CLR type name.
        /// </summary>
        public override string Name => element.CType;

    }

}
