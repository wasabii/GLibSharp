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
    class ForwardedTypeSymbolSource : ITypeSymbolSource
    {

        readonly NamespaceLibrary namespaces;
        readonly ConcurrentDictionary<Namespace, Dictionary<string, ForwardedTypeSymbol>> cache;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="namespaces"></param>
        public ForwardedTypeSymbolSource(NamespaceLibrary namespaces)
        {
            this.namespaces = namespaces ?? throw new ArgumentNullException(nameof(namespaces));

            cache = new ConcurrentDictionary<Namespace, Dictionary<string, ForwardedTypeSymbol>>();
        }

        /// <summary>
        /// Attempts to resolve a type specification to a symbol.
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="version"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public TypeSymbol ResolveSymbol(string ns, string version, string name)
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
            if (cache.GetOrAdd(m, i => GetClrInfoTypeMap(i, name)).TryGetValue(name, out var symbol))
                return symbol;

            return null;
        }

        /// <summary>
        /// Gets the specified type that has CLR info.
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        Dictionary<string, ForwardedTypeSymbol> GetClrInfoTypeMap(Namespace ns, string typeName)
        {
            if (ns is null)
                throw new ArgumentNullException(nameof(ns));
            if (typeName is null)
                throw new ArgumentNullException(nameof(typeName));

            // generates a dictionary up front of type name to forwarded type symbols
            return GetClrInfoTypes(ns).ToDictionary(i => ((IHasName)i).Name, i => new ForwardedTypeSymbol((IHasClrInfo)i));
        }

        /// <summary>
        /// Gets the specified type that has CLR info.
        /// </summary>
        /// <param name="ns"></param>
        /// <returns></returns>
        IEnumerable<Element> GetClrInfoTypes(Namespace ns)
        {
            if (ns is null)
                throw new ArgumentNullException(nameof(ns));

            return Enumerable.Empty<Element>()
                .Concat(ns.Aliases)
                .Concat(ns.BitFields)
                .Concat(ns.Classes)
                .Concat(ns.Enums)
                .Concat(ns.Interfaces)
                .Concat(ns.Primitives)
                .Concat(ns.Records)
                .Concat(ns.Unions)
                .OfType<IHasName>()
                .OfType<IHasClrInfo>()
                .Cast<Element>();
        }

    }

}