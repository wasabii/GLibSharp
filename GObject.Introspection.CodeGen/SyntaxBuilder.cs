using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;

using GObject.Introspection.Model;
using GObject.Introspection.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace GObject.Introspection.CodeGen
{

    /// <summary>
    /// Provides an entry point to generate .NET code from a set of GIR sources.
    /// </summary>
    public class SyntaxBuilder
    {

        readonly SyntaxGenerator syntax;
        readonly IntrospectionLibrary library;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="syntax"></param>
        /// <param name="library"></param>
        public SyntaxBuilder(SyntaxGenerator syntax, IntrospectionLibrary library)
        {
            this.syntax = syntax ?? throw new ArgumentNullException(nameof(syntax));
            this.library = library ?? throw new ArgumentNullException(nameof(library));
        }

        /// <summary>
        /// Exports the code required to generate the specified namespace.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public SyntaxBuilderResult ExportNamespace(string name, string version)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));
            if (version is null)
                throw new ArgumentNullException(nameof(version));

            var ns = library.ResolveNamespace(name, version);
            if (ns == null)
                throw new SyntaxBuilderException("Unable to resolve specified namespace.");

            var bd = new NamespaceBuilder();
            var rs = bd.Build(ns);

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

            return new SyntaxBuilderResult(u, new ReadOnlyCollection<SyntaxBuilderMessage>(messages));
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
            context = context.WithDebugText($"Namespace({ns})");
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
