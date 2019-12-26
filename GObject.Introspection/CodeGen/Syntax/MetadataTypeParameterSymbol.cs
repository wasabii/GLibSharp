using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using GObject.Introspection.CodeGen.Model;

namespace GObject.Introspection.CodeGen.Syntax
{

    class MetadataTypeParameterSymbol : ITypeParameterSymbol
    {

        readonly Microsoft.CodeAnalysis.ITypeParameterSymbol reference;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="reference"></param>
        public MetadataTypeParameterSymbol(Microsoft.CodeAnalysis.ITypeParameterSymbol reference)
        {
            this.reference = reference ?? throw new ArgumentNullException(nameof(reference));
        }

        public string Name => reference.Name;

        public bool IsArray => throw new NotSupportedException();

        public bool IsBlittable => throw new NotSupportedException();

        public bool IsGenericType => throw new NotSupportedException();

        public bool IsOpenGenericType => throw new NotSupportedException();

        public IReadOnlyList<ITypeParameterSymbol> TypeParameters => ImmutableList<ITypeParameterSymbol>.Empty;

        public IReadOnlyList<ITypeSymbol> TypeArguments => ImmutableList<ITypeSymbol>.Empty;

        public ITypeSymbol MakeGenericType(params ITypeSymbol[] typeArguments)
        {
            throw new NotSupportedException();
        }

    }

}
