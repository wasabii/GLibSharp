using System.Collections.Generic;

using Gir.Model;

namespace Gir.CodeGen
{

    /// <summary>
    /// Provides a mechanism to resolve a namespace from a repository.
    /// </summary>
    public interface IRepositoryProvider
    {

        /// <summary>
        /// Returns all available <see cref="Repository"/> instances.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Repository> GetRepositories();

    }

}
