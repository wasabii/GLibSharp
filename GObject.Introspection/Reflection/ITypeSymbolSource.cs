namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Provides a source to resolve GObject introspection type names.
    /// </summary>
    public interface ITypeSymbolSource
    {

        /// <summary>
        /// Resolves the specified type name from the given namespace information.
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="version"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        TypeSymbol ResolveSymbol(string ns, string version, string name);

    }

}
