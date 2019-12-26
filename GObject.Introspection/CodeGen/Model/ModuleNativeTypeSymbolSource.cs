using System;

namespace GObject.Introspection.CodeGen.Model
{

    /// <summary>
    /// Provides a native type symbol source across an existing loaded introspection library.
    /// </summary>
    class ModuleNativeTypeSymbolSource : INativeTypeSymbolSource
    {

        readonly ModuleLibrary library;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="library"></param>
        public ModuleNativeTypeSymbolSource(ModuleLibrary library)
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
        public NativeTypeSymbol ResolveSymbol(string ns, string version, string name)
        {
            if (ns is null)
                throw new ArgumentNullException(nameof(ns));
            if (version is null)
                throw new ArgumentNullException(nameof(version));
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            // recurse back into introspection library
            if (library.ResolveModule(ns, version).ResolveTypeDefByNativeName(name) is TypeDef type)
                return new ModuleNativeTypeSymbol(type);

            return null;
        }

    }

}
