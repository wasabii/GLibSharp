using System;
using System.Collections.Generic;
using System.Linq;

namespace GObject.Introspection.CodeGen.Model
{

    /// <summary>
    /// Describes a generic type that has some provided generic arguments.
    /// </summary>
    class GenericTypeSymbol : ITypeSymbol
    {

        readonly ITypeSymbol underlying;
        readonly IReadOnlyList<ITypeSymbol> typeArguments;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="underlying"></param>
        /// <param name="typeArguments"></param>
        public GenericTypeSymbol(ITypeSymbol underlying, IReadOnlyList<ITypeSymbol> typeArguments)
        {
            this.underlying = underlying ?? throw new ArgumentNullException(nameof(underlying));
            this.typeArguments = typeArguments ?? throw new ArgumentNullException(nameof(typeArguments));
        }

        public string Name => underlying.Name;

        public bool IsArray => false;

        public bool IsBlittable => false;

        public bool IsGenericType => true;

        public bool IsOpenGenericType => TypeArguments.Count(i => i != null) != TypeParameters.Count;

        public IReadOnlyList<ITypeParameterSymbol> TypeParameters => underlying.TypeParameters;

        public IReadOnlyList<ITypeSymbol> TypeArguments => typeArguments;

        public ITypeSymbol MakeGenericType(params ITypeSymbol[] typeArguments)
        {
            if (typeArguments is null)
                throw new ArgumentNullException(nameof(typeArguments));

            return new GenericTypeSymbol(this, TypeArguments.Zip(typeArguments, (i, j) => j ?? i).ToList());
        }

    }

}
