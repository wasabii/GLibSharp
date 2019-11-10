using Microsoft.CodeAnalysis;

namespace Gir.CodeGen
{

    public interface ISyntaxBuilder
    {

        /// <summary>
        /// Adds a repository source to the builder.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        ISyntaxBuilder AddSource(IRepositorySource source);

        /// <summary>
        /// Adds a namespace to be built.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        ISyntaxBuilder AddNamespace(string name);

        /// <summary>
        /// Initiates a build of the configured namespaces.
        /// </summary>
        /// <returns></returns>
        SyntaxNode Build();

    }

}
