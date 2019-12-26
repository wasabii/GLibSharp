using System;
using System.Collections.Generic;

namespace GObject.Introspection.CodeGen.Model
{

    /// <summary>
    /// Describes a reference to a type available within the reflection library.
    /// </summary>
    class ModuleTypeSymbol : ITypeSymbol
    {

        readonly TypeDef typeDef;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="typeDef"></param>
        internal ModuleTypeSymbol(TypeDef typeDef)
        {
            this.typeDef = typeDef ?? throw new ArgumentNullException(nameof(typeDef));
        }

        /// <summary>
        /// Gets the introspection type that is referenced by this symbol.
        /// </summary>
        public Type Type => typeDef.Type;

        /// <summary>
        /// Gets the qualified name of the type.
        /// </summary>
        public string Name => typeDef.Name;

        public bool IsArray => false;

        public bool IsBlittable => false;

        public bool IsGenericType => false;

        public bool IsOpenGenericType => false;

        public IReadOnlyList<ITypeParameterSymbol> TypeParameters => new List<ITypeParameterSymbol>();

        public IReadOnlyList<ITypeSymbol> TypeArguments => new List<ITypeSymbol>();

        public ITypeSymbol MakeGenericType(params ITypeSymbol[] typeArguments)
        {
            throw new InvalidOperationException();
        }

    }

}
