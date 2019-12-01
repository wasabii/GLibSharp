namespace Gir.CodeGen
{

    /// <summary>
    /// Resolves the info for a given type name.
    /// </summary>
    public interface ITypeInfoProvider
    {

        /// <summary>
        /// Resolves the info for a given type name.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        TypeInfo Resolve(TypeName type);

    }

}
