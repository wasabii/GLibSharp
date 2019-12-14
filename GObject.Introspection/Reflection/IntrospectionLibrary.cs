using System;
using System.Collections.Concurrent;

using GObject.Introspection.Library;
using GObject.Introspection.Model;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Provides the capability of resolving <see cref="IntrospectionModule"/> instances, which form a model of
    /// interaction with a loaded GObject introspected library.
    /// </summary>
    public class IntrospectionLibrary
    {

        readonly NamespaceLibrary namespaces;
        readonly TypeSymbolProvider symbols;
        readonly ConcurrentDictionary<(string, string), IntrospectionModule> modules;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="namespaces"></param>
        public IntrospectionLibrary(NamespaceLibrary namespaces)
        {
            this.namespaces = namespaces ?? throw new ArgumentNullException(nameof(namespaces));

            symbols = new TypeSymbolProvider(new IntrospectionTypeSymbolSource(this), new ForwardedTypeSymbolSource(namespaces));
            modules = new ConcurrentDictionary<(string, string), IntrospectionModule>();
        }

        /// <summary>
        /// Resolves the namespace of the given version.
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public IntrospectionModule ResolveModule(string ns, string version)
        {
            if (ns is null)
                throw new ArgumentNullException(nameof(ns));
            if (version is null)
                throw new ArgumentNullException(nameof(version));

            return modules.GetOrAdd((ns, version), i => ResolveModuleInternal(i.Item1, i.Item2));
        }

        /// <summary>
        /// Implements the logic behind Resolve.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        IntrospectionModule ResolveModuleInternal(string name, string version)
        {
            return namespaces.ResolveNamespace(name, version) is Namespace ns ? new IntrospectionModule(symbols, ns) : null;
        }

    }

}
