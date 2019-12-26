using System;
using System.Collections.Concurrent;

using GObject.Introspection.Library;
using GObject.Introspection.Library.Model;

namespace GObject.Introspection.CodeGen.Model
{

    /// <summary>
    /// Provides the capability of resolving <see cref="Module"/> instances, which form a model of
    /// interaction with a loaded GObject introspected library.
    /// </summary>
    class ModuleLibrary
    {

        readonly NamespaceLibrary namespaces;
        readonly IManagedTypeResolver resolver;

        readonly TypeSymbolProvider symbols;
        readonly NativeTypeSymbolProvider nativeSymbols;
        readonly ConcurrentDictionary<(string, string), Module> modules;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="namespaces"></param>
        /// <param name="resolver"></param>
        public ModuleLibrary(NamespaceLibrary namespaces, IManagedTypeResolver resolver)
        {
            this.namespaces = namespaces ?? throw new ArgumentNullException(nameof(namespaces));
            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));

            symbols = new TypeSymbolProvider(new ModuleTypeSymbolSource(this), new ManagedTypeSymbolSource(namespaces, resolver));
            nativeSymbols = new NativeTypeSymbolProvider(new ModuleNativeTypeSymbolSource(this), new PrimitiveNativeTypeSymbolSource(namespaces));
            modules = new ConcurrentDictionary<(string, string), Module>();
        }

        /// <summary>
        /// Resolves the namespace of the given version.
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public Module ResolveModule(string ns, string version)
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
        Module ResolveModuleInternal(string name, string version)
        {
            // attempt to resolve namespace
            if (namespaces.ResolveNamespace(name, version) is NamespaceElement ns)
                return new Module(this, resolver, symbols, nativeSymbols, ns);
            else
                return null;
        }

    }

}
