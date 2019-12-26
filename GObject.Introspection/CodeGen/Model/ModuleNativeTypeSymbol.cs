using System;

namespace GObject.Introspection.CodeGen.Model
{

    /// <summary>
    /// Describes a reference to a type available within the introspection library.
    /// </summary>
    class ModuleNativeTypeSymbol : NativeTypeSymbol
    {

        readonly TypeDef typeDef;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="typeDef"></param>
        internal ModuleNativeTypeSymbol(TypeDef typeDef)
        {
            this.typeDef = typeDef ?? throw new ArgumentNullException(nameof(typeDef));
        }

        /// <summary>
        /// Gets the introspection type that is referenced by this symbol.
        /// </summary>
        public Type Type => typeDef.Type;

        /// <summary>
        /// Gets the name of the type.
        /// </summary>
        public override string Name => typeDef.NativeName;

    }

}
