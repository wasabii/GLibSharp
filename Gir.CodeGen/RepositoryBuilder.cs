using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace Gir.CodeGen
{

    /// <summary>
    /// Provides an entry point to generate .NET code from a set of GIR sources.
    /// </summary>
    public class RepositoryBuilder : IRepositoryBuilder, IContext
    {

        readonly SyntaxGenerator syntax;
        readonly IEnumerable<ISyntaxNodeGenerator> generators;
        readonly IEnumerable<IProcessor> processors;
        readonly IList<IRepositoryXmlSource> xmlSources = new List<IRepositoryXmlSource>();
        readonly IList<(string name, string version)> namespaces = new List<(string, string)>();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="syntax"></param>
        /// <param name="generators"></param>
        /// <param name="processors"></param>
        public RepositoryBuilder(SyntaxGenerator syntax, IEnumerable<ISyntaxNodeGenerator> generators, IEnumerable<IProcessor> processors)
        {
            this.syntax = syntax;
            this.generators = generators;
            this.processors = processors;
        }

        /// <summary>
        /// Adds an input GIR repository file to the builder.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public IRepositoryBuilder AddSource(IRepositoryXmlSource source)
        {
            xmlSources.Add(source);
            return this;
        }

        /// <summary>
        /// Adds a namespace to be built.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public IRepositoryBuilder AddNamespace(string name, string version)
        {
            namespaces.Add((name, version));
            return this;
        }

        /// <summary>
        /// Initiates a build of the configured namespaces.
        /// </summary>
        /// <returns></returns>
        public SyntaxNode Build()
        {
            return syntax.CompilationUnit(namespaces.Select(i => BuildNamespace(i.name, i.version)));
        }

        /// <summary>
        /// Initiates a build for the given namespace.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        SyntaxNode BuildNamespace(string name, string version) => generators
            .SelectMany(i => i.BuildNamespace(this, name, version))
            .FirstOrDefault(i => i != null);

        /// <summary>
        /// Gets the current <see cref="SyntaxGenerator"/>.
        /// </summary>
        SyntaxGenerator IContext.Syntax => syntax;

        /// <summary>
        /// Resolves the element associated with the given namespace.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        XElement? IContext.ResolveNamespace(string name, string version) =>
            xmlSources
                .Select(i => i.ResolveNamespace(name, version))
                .FirstOrDefault(i => i != null);

        /// <summary>
        /// Attempts to build the syntax nodes for the given element.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        IEnumerable<SyntaxNode> IContext.Build(XElement element) => processors
            .SelectMany(i => i.Build(this, element))
            .Select(i => processors.Aggregate(i, (n, p) => p.Adjust(this, element, n)));

    }

}
