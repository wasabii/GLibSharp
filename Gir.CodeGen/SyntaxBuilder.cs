using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Gir.Model;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace Gir.CodeGen
{

    /// <summary>
    /// Provides an entry point to generate .NET code from a set of GIR sources.
    /// </summary>
    partial class SyntaxBuilder : ISyntaxBuilder
    {

        readonly SyntaxGenerator syntax;
        readonly IEnumerable<ISyntaxNodeBuilder> builders;
        readonly List<IRepositorySource> sources = new List<IRepositorySource>();
        readonly List<string> namespaces = new List<string>();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="syntax"></param>
        /// <param name="builders"></param>
        public SyntaxBuilder(SyntaxGenerator syntax, IEnumerable<ISyntaxNodeBuilder> builders)
        {
            this.syntax = syntax ?? throw new ArgumentNullException(nameof(syntax));
            this.builders = builders ?? throw new ArgumentNullException(nameof(builders));
        }

        /// <summary>
        /// Adds an input GIR repository file to the builder.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public ISyntaxBuilder AddSource(IRepositorySource source)
        {
            sources.Add(source);
            return this;
        }

        /// <summary>
        /// Adds a namespace to be built.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public ISyntaxBuilder AddNamespace(string name)
        {
            namespaces.Add(name);
            return this;
        }

        /// <summary>
        /// Initiates a build of the configured namespaces.
        /// </summary>
        /// <returns></returns>
        public SyntaxNode Build()
        {
            var z = ImmutableList<string>.Empty;
            var r = new RepositoryProvider(sources);
            var p = new ClrTypeInfoProvider(new[] { new ClrTypeInfoRepositorySource(r) });
            var c = new SymbolBuilderContext(null, z, this, syntax, r, p, ImmutableList<object>.Empty);
            var l = namespaces.SelectMany(i => r.GetRepositories().SelectMany(j => j.Namespaces.Where(k => k.Name == i).Select(k => new { Repository = j, Namespace = k })));
            var s = l.SelectMany(i => BuildNamespace(c, i.Repository, i.Namespace)).ToList();
            var u = syntax.CompilationUnit(s);

            switch (u)
            {
                case Microsoft.CodeAnalysis.CSharp.Syntax.CompilationUnitSyntax cs:
                    u = cs = cs.WithAttributeLists(
                        new SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax>(
                            s.OfType<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax>()));
                    break;
                case Microsoft.CodeAnalysis.VisualBasic.Syntax.CompilationUnitSyntax vb:
                default:
                    throw new NotImplementedException();
            }

            return u;
        }

        /// <summary>
        /// Initiates a build for the given namespace.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="rs"></param>
        /// <param name="ns"></param>
        /// <returns></returns>
        IEnumerable<SyntaxNode> BuildNamespace(IContext context, Repository rs, Namespace ns)
        {
            context = ResolveNamespaces(context, rs).Aggregate(context, (a, b) => a.WithImport(b.Name));
            return BuildElement(context, ns);
        }

        /// <summary>
        /// Expands all required namespaces present in the given repository.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="repository"></param>
        /// <returns></returns>
        IEnumerable<Namespace> ResolveNamespaces(IContext context, Repository repository)
        {
            foreach (var include in repository.Includes)
                foreach (var (r, ns) in ResolveNamespace(context, include.Name))
                    foreach (var i in ResolveNamespaces(context, r))
                        yield return i;

            foreach (var ns in repository.Namespaces)
                yield return ns;
        }

        /// <summary>
        /// Resolves the namespaces matching the specified name.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        IEnumerable<(Repository, Namespace)> ResolveNamespace(IContext context, string name)
        {
            return context.Repositories.GetRepositories()
                .SelectMany(i => i.Namespaces
                    .Select<Namespace, (Repository Repository, Namespace Namespace)>(j => (i, j)))
                .Where(i => i.Namespace.Name == name);
        }

        /// <summary>
        /// Initiates a build for the given element.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        internal IEnumerable<SyntaxNode> BuildElement(IContext context, Element element)
        {
            // initiate build of all available symbols in the namespace
            // pass results back through builders for adjustment
            return builders
                .SelectMany(builder => builder.Build(context, element))
                .Select(node => builders.Aggregate(node, (n, b) => b.Adjust(context, element, n) ?? n));
        }

    }

}
