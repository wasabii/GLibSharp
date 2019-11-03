using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;

namespace Gir.CodeGen
{

    /// <summary>
    /// Default implementation of <see cref="ISyntaxNodeGenerator"/>.
    /// </summary>
    class DefaultSyntaxNodeGenerator : ISyntaxNodeGenerator
    {

        readonly IEnumerable<IProcessor> processors;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="processors"></param>
        public DefaultSyntaxNodeGenerator(IEnumerable<IProcessor> processors)
        {
            this.processors = processors;
        }

        public IEnumerable<SyntaxNode> BuildNamespace(IContext context, string namespace_, string version)
        {
            var r = context.ResolveNamespace(namespace_, version);
            if (r == null)
                throw new InvalidOperationException($"Could not resolve namespace {namespace_}-{version}.");

            return context.Build(r);
        }

    }

}
