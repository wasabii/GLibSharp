using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using GObject.Introspection.Model;

namespace GObject.Introspection.Emit
{

    /// <summary>
    /// Provides available managed type info for lookup by full type name.
    /// </summary>
    public class TypeInfoResolver
    {

        readonly IEnumerable<ITypeInfoSource> sources;
        readonly ConcurrentDictionary<TypeSymbol, TypeInfo> cache;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="sources"></param>
        public TypeInfoResolver(IEnumerable<ITypeInfoSource> sources)
        {
            this.sources = sources ?? throw new ArgumentNullException(nameof(sources));

            cache = new ConcurrentDictionary<TypeSymbol, TypeInfo>();
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="sources"></param>
        public TypeInfoResolver(params ITypeInfoSource[] sources) :
            this(sources.AsEnumerable())
        {

        }

        /// <summary>
        /// Looks up a <see cref="TypeInfo"/> given the specified full GObject name.
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public TypeInfo Resolve(TypeSymbol symbol)
        {
            if (symbol is null)
                throw new ArgumentNullException(nameof(symbol));

            return cache.GetOrAdd(symbol, i => ResolveInternal(i));
        }

        TypeInfo ResolveInternal(TypeSymbol symbol)
        {
            return sources.Select(i => i.ResolveTypeInfo(symbol)).FirstOrDefault(i => i != null);
        }

    }

}
