using System.Reflection;

using GObject.Introspection.Model;

namespace GObject.Introspection.Emit
{

    /// <summary>
    /// Provides a source to resolve <see cref="TypeInfo"/> instances.
    /// </summary>
    public interface ITypeInfoSource
    {

        /// <summary>
        /// Resolves the specified type symbol.
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        TypeInfo ResolveTypeInfo(TypeSymbol symbol);

    }

}
