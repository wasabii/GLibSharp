using Microsoft.CodeAnalysis;

namespace Gir.CodeGen
{

    public interface IRepositoryBuilder
    {

        /// <summary>
        /// Adds a repository source to the builder.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        IRepositoryBuilder AddSource(IRepositoryXmlSource source);

        /// <summary>
        /// Adds a namespace to be built.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        IRepositoryBuilder AddNamespace(string name, string version);

        /// <summary>
        /// Initiates a build of the configured namespaces.
        /// </summary>
        /// <returns></returns>
        SyntaxNode Build();

    }

}
