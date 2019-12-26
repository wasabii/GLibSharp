using System;
using System.Collections.Generic;
using System.Linq;

using GObject.Introspection.CodeGen.Model;

using Microsoft.CodeAnalysis;

namespace GObject.Introspection.CodeGen.Syntax
{

    /// <summary>
    /// Provides a <see cref="IManagedTypeResolver"/> from Roslyn assembly symbols.
    /// </summary>
    public class ManagedTypeResolver : IManagedTypeResolver
    {

        readonly IEnumerable<IAssemblySymbol> assemblies;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="assemblies"></param>
        public ManagedTypeResolver(IEnumerable<IAssemblySymbol> assemblies)
        {
            this.assemblies = assemblies ?? throw new ArgumentNullException(nameof(assemblies));
        }

        public IManagedTypeReference Resolve(string name)
        {
            return assemblies
                .SelectMany(i => Resolve(i.GlobalNamespace, name))
                .Select(i => new MetadataTypeReference(i))
                .FirstOrDefault(i => i != null);
        }

        IEnumerable<ITypeSymbol> Resolve(INamespaceOrTypeSymbol parent, string name)
        {
            foreach (var m in parent.GetMembers().OfType<INamespaceOrTypeSymbol>())
            {
                // check if type matches
                if (m is ITypeSymbol t && t.CanBeReferencedByName && t.ContainingNamespace.Name + "." + t.Name == name)
                    yield return t;

                // recurse into members of type
                foreach (var i in Resolve(m, name))
                    yield return i;
            }
        }

    }

}
