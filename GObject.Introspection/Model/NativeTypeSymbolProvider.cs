using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace GObject.Introspection.Model
{

    /// <summary>
    /// Provides available native type symbols for lookup by full introspected name.
    /// </summary>
    class NativeTypeSymbolProvider
    {

        readonly IEnumerable<INativeTypeSymbolSource> sources;
        readonly ConcurrentDictionary<(string, string, string), NativeTypeSymbol> cache;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="sources"></param>
        public NativeTypeSymbolProvider(IEnumerable<INativeTypeSymbolSource> sources)
        {
            this.sources = sources ?? throw new ArgumentNullException(nameof(sources));

            cache = new ConcurrentDictionary<(string, string, string), NativeTypeSymbol>();
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="sources"></param>
        public NativeTypeSymbolProvider(params INativeTypeSymbolSource[] sources) :
            this(sources.AsEnumerable())
        {

        }
        
        /// <summary>
        /// Attempts to resolve a native type symbol referencing a type by the given introspection name.
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="version"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public NativeTypeSymbol Resolve(string ns, string version, string name)
        {
            if (ns is null)
                throw new ArgumentNullException(nameof(ns));
            if (version is null)
                throw new ArgumentNullException(nameof(version));
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            return cache.GetOrAdd((ns, version, name), i => ResolveInternal(i.Item1, i.Item2, i.Item3));
        }

        NativeTypeSymbol ResolveInternal(string ns, string version, string name)
        {
            return sources.Select(i => i.ResolveSymbol(ns, version, name)).FirstOrDefault(i => i != null);
        }

    }

}
