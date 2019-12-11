using System;
using System.Collections.Concurrent;
using System.Linq;

using GObject.Introspection.Library;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Provides the capability of resolving <see cref="IntrospectionNamespace"/> instances, which form a model of
    /// interaction with a loaded GObject introspected library.
    /// </summary>
    public class IntrospectionLibrary
    {

        readonly NamespaceLibrary namespaces;
        readonly TypeSymbolProvider symbols;
        readonly ConcurrentDictionary<(string, string), IntrospectionNamespace> cache;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="namespaces"></param>
        public IntrospectionLibrary(NamespaceLibrary namespaces)
        {
            this.namespaces = namespaces ?? throw new ArgumentNullException(nameof(namespaces));

            symbols = new TypeSymbolProvider(new RecursiveTypeSymbolSource(this), new ForwardedTypeSymbolSource(namespaces));
            cache = new ConcurrentDictionary<(string, string), IntrospectionNamespace>();
        }

        /// <summary>
        /// Resolves the namespace of the given version.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public IntrospectionNamespace ResolveNamespace(string name, string version)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));
            if (version is null)
                throw new ArgumentNullException(nameof(version));

            return cache.GetOrAdd((name, version), i => ResolveNamespaceInternal(i.Item1, i.Item2));
        }

        /// <summary>
        /// Implements the logic behind Resolve.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        IntrospectionNamespace ResolveNamespaceInternal(string name, string version)
        {
            // resolve namespace model
            var model = namespaces.ResolveNamespace(name, version);
            if (model == null)
                return null;

            // imports derived from repository of namespace, followed by self
            var imports = model.Repository.Includes
                .Select(i => (i.Name, i.Version))
                .Append((model.Name, model.Version))
                .ToList();

            // namespace context configuration
            var context = new IntrospectionContext(symbols, imports, name);

            // generate new namespace
            return new IntrospectionNamespace(context, model);
        }

    }

}
