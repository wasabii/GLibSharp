using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using GObject.Introspection.Library;
using GObject.Introspection.Xml;

namespace GObject.Introspection.Model
{

    /// <summary>
    /// Resolves type symbols from forwarded types.
    /// </summary>
    class ManagedTypeSymbolSource : ITypeSymbolSource
    {

        readonly NamespaceLibrary namespaces;
        readonly IManagedTypeResolver resolver;
        readonly ConcurrentDictionary<NamespaceElement, Dictionary<string, ManagedTypeSymbol>> cache;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="namespaces"></param>
        public ManagedTypeSymbolSource(NamespaceLibrary namespaces, IManagedTypeResolver resolver)
        {
            this.namespaces = namespaces ?? throw new ArgumentNullException(nameof(namespaces));
            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));

            cache = new ConcurrentDictionary<NamespaceElement, Dictionary<string, ManagedTypeSymbol>>();
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
        Dictionary<string, ManagedTypeSymbol> GetClrInfoTypeMap(NamespaceElement ns, string typeName)
        {
            if (ns is null)
                throw new ArgumentNullException(nameof(ns));
            if (typeName is null)
                throw new ArgumentNullException(nameof(typeName));

            // generates a dictionary up front of type name to forwarded type symbols
            return GetClrInfoTypes(ns).ToDictionary(i => ((IHasName)i).Name, i => GetTypeSymbol(((IHasClrInfo)i).ClrInfo.Type));
        }

        /// <summary>
        /// Attempts to load the managed type symbol by name from the references.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        ManagedTypeSymbol GetTypeSymbol(string name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            var type = resolver.Resolve(name);
            if (type != null)
                return ManagedTypeSymbol.FromType(type);

            return null;
        }

        /// <summary>
        /// Gets the specified type that has CLR info.
        /// </summary>
        /// <param name="ns"></param>
        /// <returns></returns>
        IEnumerable<Element> GetClrInfoTypes(NamespaceElement ns)
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
                .Where(i => i.ClrInfo?.Type != null)
                .Cast<Element>();
        }

    }

}