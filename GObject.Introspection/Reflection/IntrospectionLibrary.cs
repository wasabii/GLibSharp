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
        readonly IManagedTypeResolver resolver;

        readonly TypeSymbolProvider symbols;
        readonly NativeTypeSymbolProvider nativeSymbols;
        readonly ConcurrentDictionary<(string, string), IntrospectionModule> modules;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="namespaces"></param>
        /// <param name="resolver"></param>
        public IntrospectionLibrary(NamespaceLibrary namespaces, IManagedTypeResolver resolver)
        {
            this.namespaces = namespaces ?? throw new ArgumentNullException(nameof(namespaces));
            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));

            symbols = new TypeSymbolProvider(new IntrospectionTypeSymbolSource(this), new ManagedTypeSymbolSource(namespaces, resolver));
            nativeSymbols = new NativeTypeSymbolProvider(new IntrospectionNativeTypeSymbolSource(this), new PrimitiveNativeTypeSymbolSource(namespaces));
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
            // attempt to resolve namespace
            if (namespaces.ResolveNamespace(name, version) is Namespace ns)
                return new IntrospectionModule(resolver, symbols, nativeSymbols, ns);
            else
                return null;
        }

    }

}
