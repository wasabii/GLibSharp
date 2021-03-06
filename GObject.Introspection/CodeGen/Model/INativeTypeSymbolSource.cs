﻿namespace GObject.Introspection.CodeGen.Model
{

    /// <summary>
    /// Provides a source to resolve native GObject introspection type names.
    /// </summary>
    interface INativeTypeSymbolSource
    {

        /// <summary>
        /// Resolves the specified native type name from the given namespace information.
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="version"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        NativeTypeSymbol ResolveSymbol(string ns, string version, string name);

    }

}
