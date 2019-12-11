using GObject.Introspection.Model;

namespace GObject.Introspection.Library
{

    /// <summary>
    /// Provides a mechanism to resolve a namespace.
    /// </summary>
    public interface INamespaceSource
    {

        /// <summary>
        /// Resolves a specific namespace.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        Namespace Resolve(string name, string version);

    }

}
