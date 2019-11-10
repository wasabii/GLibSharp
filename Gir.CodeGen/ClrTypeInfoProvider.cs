using System;
using System.Collections.Generic;
using System.Linq;

namespace Gir.CodeGen
{

    /// <summary>
    /// Provides the known <see cref="ClrTypeInfo"/> instances across multiple sources.
    /// </summary>
    class ClrTypeInfoProvider : IClrTypeInfoProvider
    {

        readonly IEnumerable<IClrTypeInfoSource> sources;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="sources"></param>
        public ClrTypeInfoProvider(IEnumerable<IClrTypeInfoSource> sources)
        {
            this.sources = sources ?? throw new ArgumentNullException(nameof(sources));
        }

        public ClrTypeInfo Resolve(GirTypeName type)
        {
            return sources.Select(i => i.Resolve(type)).FirstOrDefault(i => i != null);
        }

    }

}
