using System;
using System.Collections.Generic;
using System.Linq;

using GObject.Introspection.CodeGen.Model;

using Microsoft.CodeAnalysis;

namespace GObject.Introspection.CodeGen.Syntax
{

    /// <summary>
    /// Describes a type symbol refering to an existing CLR type.
    /// </summary>
    class MetadataTypeSymbol : Model.ITypeSymbol
    {

        readonly INamedTypeSymbol reference;
        readonly List<Model.ITypeParameterSymbol> typeParameters;
        readonly List<Model.ITypeSymbol> typeArguments;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="reference"></param>
        public MetadataTypeSymbol(INamedTypeSymbol reference)
        {
            this.reference = reference ?? throw new ArgumentNullException(nameof(reference));

            typeParameters = reference.TypeParameters.Select(i => (Model.ITypeParameterSymbol)new MetadataTypeParameterSymbol(i)).ToList();
            typeArguments = new List<Model.ITypeSymbol>();
        }

        /// <summary>
        /// Gets the qualified CLR type name.
        /// </summary>
        public string Name => reference.ContainingNamespace + "." + reference.Name;

        /// <summary>
        /// Returns whether or not the referenced type is an array.
        /// </summary>
        public bool IsArray => false;

        /// <summary>
        /// Gets whether or not the type is blittable.
        /// </summary>
        public bool IsBlittable => false;

        /// <summary>
        /// Gets whether the type is a generic type.
        /// </summary>
        public bool IsGenericType => reference.IsGenericType;

        /// <summary>
        /// Gets whether the type is an unbound generic.
        /// </summary>
        public bool IsOpenGenericType => TypeArguments.Count(i => i != null) != TypeParameters.Count;

        /// <summary>
        /// Gets the type parameters.
        /// </summary>
        public IReadOnlyList<Model.ITypeParameterSymbol> TypeParameters => typeParameters;

        /// <summary>
        /// Gets the type arguments.
        /// </summary>
        public IReadOnlyList<Model.ITypeSymbol> TypeArguments => typeArguments;

        /// <summary>
        /// Makes a generic type from the current open generic type.
        /// </summary>
        /// <param name="typeArguments"></param>
        /// <returns></returns>
        public Model.ITypeSymbol MakeGenericType(params Model.ITypeSymbol[] typeArguments)
        {
            if (typeArguments is null)
                throw new ArgumentNullException(nameof(typeArguments));
            if (typeArguments.Length != typeParameters.Count)
                throw new ArgumentException("Type argument count does not match arity.");

            var t = new List<Model.ITypeSymbol>(typeParameters.Count);
            for (var i = 0; i < typeParameters.Count; i++)
                t.Add(typeArguments[i] ?? (this.typeArguments.Count > i ? this.typeArguments[i] : null));

            return new GenericTypeSymbol(this, t);
        }

    }

}
