using System;
using System.Reflection;

using GObject.Introspection.CodeGen.Model;

using Microsoft.CodeAnalysis;

namespace GObject.Introspection.CodeGen.Syntax
{

    /// <summary>
    /// Wraps a Roslyn <see cref="ITypeSymbol"/> as a <see cref="IManagedTypeReference"/>.
    /// </summary>
    class MetadataTypeReference : IManagedTypeReference
    {

        readonly ITypeSymbol symbol;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="symbol"></param>
        public MetadataTypeReference(ITypeSymbol symbol)
        {
            this.symbol = symbol ?? throw new ArgumentNullException(nameof(symbol));
        }

        public AssemblyName AssemblyName => new AssemblyName(symbol.ContainingAssembly.Identity.Name);

        public string Name => symbol.ContainingNamespace + "." + symbol.Name;

        public bool IsArray => false;

        public bool IsBlittable => false;

    }

}
