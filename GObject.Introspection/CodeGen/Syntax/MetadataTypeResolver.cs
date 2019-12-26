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
    class MetadataTypeResolver : IManagedTypeResolver
    {

        /// <summary>
        /// Returns all of the types from the given symbol.
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        static IEnumerable<INamedTypeSymbol> GetTypes(INamespaceOrTypeSymbol parent)
        {
            foreach (var m in parent.GetMembers().OfType<INamespaceOrTypeSymbol>())
            {
                // check if type matches
                if (m is INamedTypeSymbol t && t.CanBeReferencedByName)
                    yield return t;

                // recurse into members of type
                foreach (var i in GetTypes(m))
                    yield return i;
            }
        }

        readonly Dictionary<string, MetadataTypeSymbol> typeMap;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="assemblies"></param>
        public MetadataTypeResolver(IEnumerable<IAssemblySymbol> assemblies)
        {
            if (assemblies is null)
                throw new ArgumentNullException(nameof(assemblies));

            // cache away type names
            typeMap = assemblies
                .SelectMany(i => GetTypes(i.GlobalNamespace))
                .GroupBy(i => i.ContainingNamespace + "." + i.MetadataName)
                .OrderBy(i => i.Key)
                .Select(i => i.First())
                .ToDictionary(i => i.ContainingNamespace + "." + i.MetadataName, i => new MetadataTypeSymbol(i));
        }

        public Model.ITypeSymbol Resolve(string name)
        {
            return typeMap.TryGetValue(name, out var r) ? r : null;
        }

    }

}
