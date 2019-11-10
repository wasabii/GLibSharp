using System.Collections.Generic;

using Gir.Model;

namespace Gir.CodeGen
{

    /// <summary>
    /// Provides a mechanism to resolve a namespace from a repository.
    /// </summary>
    public interface IRepositorySource
    {

        /// <summary>
        /// Gets all available repository information from the source.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Repository> GetRepositories();

    }

}
