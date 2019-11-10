using System;
using System.Collections.Generic;
using System.Linq;

using Gir.Model;

namespace Gir.CodeGen
{

    /// <summary>
    /// Provides the known <see cref="Repository"/> instances across multiple sources.
    /// </summary>
    class RepositoryProvider : IRepositoryProvider
    {

        readonly IEnumerable<IRepositorySource> sources;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="sources"></param>
        public RepositoryProvider(IEnumerable<IRepositorySource> sources)
        {
            this.sources = sources ?? throw new ArgumentNullException(nameof(sources));
        }

        public IEnumerable<Repository> GetRepositories()
        {
            return sources.SelectMany(i => i.GetRepositories());
        }

    }

}
