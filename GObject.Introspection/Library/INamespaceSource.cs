using GObject.Introspection.Xml;

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
        NamespaceElement Resolve(string name, string version);

    }

}
