namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Provides resolution of managed type.
    /// </summary>
    public interface IManagedTypeResolver
    {

        /// <summary>
        /// Resolves a managed type by the specified managed type name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IManagedTypeReference Resolve(string name);

    }

}
