using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using GObject.Introspection.Library.Model;

namespace GObject.Introspection.Library
{

    /// <summary>
    /// Provides the capability to resolve across multiple namespace source instances.
    /// </summary>
    public class NamespaceLibrary
    {

        readonly IEnumerable<INamespaceSource> sources;
        readonly ConcurrentDictionary<(string, string), NamespaceElement> cache;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="sources"></param>
        public NamespaceLibrary(IEnumerable<INamespaceSource> sources)
        {
            this.sources = sources ?? throw new ArgumentNullException(nameof(sources));
            this.sources = this.sources.Append(new CNamespaceSource());

            cache = new ConcurrentDictionary<(string, string), NamespaceElement>();
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="sources"></param>
        public NamespaceLibrary(params INamespaceSource[] sources) :
            this(sources.AsEnumerable())
        {

        }

        /// <summary>
        /// Resolves the <see cref="NamespaceElement"/> record with the given name and version.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public NamespaceElement ResolveNamespace(string name, string version)
        {
            return cache.GetOrAdd((name, version), i => ResolveNamespaceInternal(i.Item1, i.Item2));
        }

        NamespaceElement ResolveNamespaceInternal(string name, string version)
        {
            return sources.Select(i => i.Resolve(name, version)).FirstOrDefault(i => i != null);
        }

    }

}
