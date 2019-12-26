namespace GObject.Introspection.CodeGen.Model
{

    /// <summary>
    /// Provides a source to resolve GObject introspection type names.
    /// </summary>
    interface ITypeSymbolSource
    {

        /// <summary>
        /// Resolves the specified type name from the given namespace information.
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="version"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        ITypeSymbol ResolveSymbol(string ns, string version, string name);

    }

}
