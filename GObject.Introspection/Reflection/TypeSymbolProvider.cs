using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Provides available managed type symbols for lookup by full introspected name.
    /// </summary>
    class TypeSymbolProvider
    {

        readonly IEnumerable<ITypeSymbolSource> sources;
        readonly ConcurrentDictionary<(string, string, string), TypeSymbol> cache;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="sources"></param>
        public TypeSymbolProvider(IEnumerable<ITypeSymbolSource> sources)
        {
            this.sources = sources ?? throw new ArgumentNullException(nameof(sources));

            cache = new ConcurrentDictionary<(string, string, string), TypeSymbol>();
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="sources"></param>
        public TypeSymbolProvider(params ITypeSymbolSource[] sources) :
            this(sources.AsEnumerable())
        {

        }
        
        /// <summary>
        /// Attempts to resolve a type symbol referencing a type by the given introspection name.
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="version"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public TypeSymbol Resolve(string ns, string version, string name)
        {
            if (ns is null)
                throw new ArgumentNullException(nameof(ns));
            if (version is null)
                throw new ArgumentNullException(nameof(version));
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            return cache.GetOrAdd((ns, version, name), i => ResolveInternal(i.Item1, i.Item2, i.Item3));
        }

        TypeSymbol ResolveInternal(string ns, string version, string name)
        {
            return sources.Select(i => i.ResolveSymbol(ns, version, name)).FirstOrDefault(i => i != null);
        }

    }

}
