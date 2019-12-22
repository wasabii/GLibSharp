using System;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Provides a type symbol source across an existing loaded introspection library.
    /// </summary>
    class IntrospectionTypeSymbolSource : ITypeSymbolSource
    {

        readonly IntrospectionLibrary library;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="library"></param>
        public IntrospectionTypeSymbolSource(IntrospectionLibrary library)
        {
            this.library = library ?? throw new ArgumentNullException(nameof(library));
        }

        /// <summary>
        /// Resolves the specified type name from the given namespace information.
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

            // recurse back into introspection library
            if (library.ResolveModule(ns, version).ResolveTypeDef(name) is IntrospectionTypeDef type)
                return new IntrospectionTypeSymbol(type);

            return null;
        }

    }

}
