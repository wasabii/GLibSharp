using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using GObject.Introspection.Library;
using GObject.Introspection.Model;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Resolves type symbols from forwarded types.
    /// </summary>
    class PrimitiveNativeTypeSymbolSource : INativeTypeSymbolSource
    {

        readonly NamespaceLibrary namespaces;
        readonly ConcurrentDictionary<Namespace, Dictionary<string, PrimitiveNativeTypeSymbol>> cache;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="namespaces"></param>
        public PrimitiveNativeTypeSymbolSource(NamespaceLibrary namespaces)
        {
            this.namespaces = namespaces ?? throw new ArgumentNullException(nameof(namespaces));

            cache = new ConcurrentDictionary<Namespace, Dictionary<string, PrimitiveNativeTypeSymbol>>();
        }

        /// <summary>
        /// Attempts to resolve a native type specification to a symbol.
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="version"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public NativeTypeSymbol ResolveSymbol(string ns, string version, string name)
        {
            if (ns is null)
                throw new ArgumentNullException(nameof(ns));
            if (version is null)
                throw new ArgumentNullException(nameof(version));
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            // find namespace entry
            var m = namespaces.ResolveNamespace(ns, version);
            if (m == null)
                return null;

            // attempt to resolve type to symbol
            if (cache.GetOrAdd(m, i => GetCTypeMap(i, name)).TryGetValue(name, out var symbol))
                return symbol;

            return null;
        }

        /// <summary>
        /// Gets the specified type that has CLR info.
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        Dictionary<string, PrimitiveNativeTypeSymbol> GetCTypeMap(Namespace ns, string typeName)
        {
            if (ns is null)
                throw new ArgumentNullException(nameof(ns));
            if (typeName is null)
                throw new ArgumentNullException(nameof(typeName));

            // generates a dictionary up front of type name to forwarded type symbols
            return GetCTypes(ns).ToDictionary(i => ((IHasName)i).Name, i => new PrimitiveNativeTypeSymbol(((IHasCType)i)));
        }

        /// <summary>
        /// Gets the specified type that has CLR info.
        /// </summary>
        /// <param name="ns"></param>
        /// <returns></returns>
        IEnumerable<Element> GetCTypes(Namespace ns)
        {
            if (ns is null)
                throw new ArgumentNullException(nameof(ns));

            return Enumerable.Empty<Element>()
                .Concat(ns.Primitives)
                .OfType<IHasName>()
                .OfType<IHasCType>()
                .Where(i => i.CType != null)
                .Cast<Element>();
        }

    }

}
