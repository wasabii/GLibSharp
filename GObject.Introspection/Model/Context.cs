using System;
using System.Collections.Generic;

using GObject.Introspection.Xml;

namespace GObject.Introspection.Model
{

    /// <summary>
    /// Describes services available to an introspection element.
    /// </summary>
    class Context
    {

        readonly Module module;
        readonly IManagedTypeResolver resolver;
        readonly TypeSymbolProvider symbols;
        readonly NativeTypeSymbolProvider nativeSymbols;
        readonly IList<(string Namespace, string Version)> imports;
        readonly string current;
        readonly TypeFactory factory;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="resolver"></param>
        /// <param name="symbols"></param>
        /// <param name="nativeSymbols"></param>
        /// <param name="imports"></param>
        /// <param name="current"></param>
        internal Context(
            Module module,
            IManagedTypeResolver resolver,
            TypeSymbolProvider symbols,
            NativeTypeSymbolProvider nativeSymbols,
            IList<(string, string)> imports,
            string current)
        {
            this.module = module ?? throw new ArgumentNullException(nameof(module));
            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            this.symbols = symbols ?? throw new ArgumentNullException(nameof(symbols));
            this.nativeSymbols = nativeSymbols ?? throw new ArgumentNullException(nameof(nativeSymbols));
            this.imports = imports ?? throw new ArgumentNullException(nameof(imports));
            this.current = current;

            factory = new TypeFactory(this);
        }

        /// <summary>
        /// Gets the current module.
        /// </summary>
        public Module Module => module;

        /// <summary>
        /// Gets the current namespace of the context.
        /// </summary>
        public string CurrentNamespace => current;

        /// <summary>
        /// Attempts to resolve the given full or partial type name given the context.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public TypeSymbol ResolveSymbol(string name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            // some well known type names
            if (name == "" || name == "none")
                return null;

            // type name might be qualified initially
            if (QualifiedTypeName.IsQualified(name))
            {
                var qualifiedName = QualifiedTypeName.Parse(name);

                // check matching namespaces in reverse order (duplicates might exist by version)
                for (var i = imports.Count - 1; i >= 0; i--)
                    if (symbols.Resolve(imports[i].Namespace, imports[i].Version, qualifiedName.Name) is TypeSymbol s)
                        return s;

                // could not find, return null
                return null;
            }

            // check the imported namespaces in reverse order
            for (var i = imports.Count - 1; i >= 0; i--)
                if (symbols.Resolve(imports[i].Namespace, imports[i].Version, name) is TypeSymbol s)
                    return s;

            // could not locate, return null
            return null;
        }

        /// <summary>
        /// Attempts to resolve the given full or partial type name given the context.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public NativeTypeSymbol ResolveNativeSymbol(string name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            // some well known type names
            if (name == "" || name == "none")
                return null;

            // check the imported namespaces in reverse order
            for (var i = imports.Count - 1; i >= 0; i--)
                if (nativeSymbols.Resolve(imports[i].Namespace, imports[i].Version, name) is NativeTypeSymbol s)
                    return s;

            // could not locate, return null
            return null;
        }

        /// <summary>
        /// Resolves a type symbol for the specified managed type.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public TypeSymbol ResolveManagedSymbol(string name)
        {
            var type = resolver.Resolve(name);
            if (type != null)
                return new ManagedTypeSymbol(resolver.Resolve(name));

            return null;
        }

        /// <summary>
        /// Creates a <see cref="Type"/> based on the given element.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public TypeDef CreateType(Element element)
        {
            return factory.CreateType(element);
        }

    }

}
