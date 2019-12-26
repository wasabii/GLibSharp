namespace GObject.Introspection.CodeGen.Model
{

    /// <summary>
    /// Provides resolution of managed type.
    /// </summary>
    interface IManagedTypeResolver
    {

        /// <summary>
        /// Resolves a managed type by the specified managed type name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        ITypeSymbol Resolve(string name);

    }

}
