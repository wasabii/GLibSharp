using System;
using System.Collections.Generic;
using System.Linq;

namespace Gir.CodeGen
{

    /// <summary>
    /// Provides the known <see cref="TypeInfo"/> instances across multiple sources.
    /// </summary>
    class TypeInfoProvider : ITypeInfoProvider
    {

        readonly IEnumerable<ITypeInfoSource> sources;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="sources"></param>
        public TypeInfoProvider(IEnumerable<ITypeInfoSource> sources)
        {
            this.sources = sources ?? throw new ArgumentNullException(nameof(sources));
        }

        public TypeInfo Resolve(TypeName type)
        {
            return sources.Select(i => i.Resolve(type)).FirstOrDefault(i => i != null);
        }

    }

}
