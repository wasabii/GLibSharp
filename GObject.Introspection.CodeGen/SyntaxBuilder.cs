using System;
using System.Collections.Generic;
using System.Linq;

using GObject.Introspection.Model;

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
        readonly ModuleLibrary library;
        readonly IEnumerable<ISyntaxNodeBuilder> builders;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="syntax"></param>
        /// <param name="library"></param>
        /// <param name="builders"></param>
        public SyntaxBuilder(SyntaxGenerator syntax, ModuleLibrary library, IEnumerable<ISyntaxNodeBuilder> builders)
        {
            this.syntax = syntax ?? throw new ArgumentNullException(nameof(syntax));
            this.library = library ?? throw new ArgumentNullException(nameof(library));
            this.builders = builders ?? throw new ArgumentNullException(nameof(builders));
        }

        /// <summary>
        /// Exports the code required to generate the specified namespace.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public SyntaxNode ExportNamespace(string name, string version)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));
            if (version is null)
                throw new ArgumentNullException(nameof(version));

            var ns = library.ResolveModule(name, version);
            if (ns == null)
                throw new SyntaxBuilderException("Unable to resolve specified namespace.");

            // build types in each emitted namespace
            var s = ns.Types.GroupBy(i => "").SelectMany(i => BuildNamespace(i.Key, i));
            var u = syntax.CompilationUnit(s);
            if (u == null)
                throw new SyntaxBuilderException("Unable to build node for namespace.");

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
        /// Builds the specified types within the specified namespace name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        IEnumerable<SyntaxNode> BuildNamespace(string name, IEnumerable<Model.Type> types)
        {
            yield return syntax.NamespaceDeclaration(name, types.SelectMany(i => BuildNode(i)));
        }

        /// <summary>
        /// Initiates a build for the given node.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        internal IEnumerable<SyntaxNode> BuildNode(IIntrospectionNode node)
        {
            return builders.SelectMany(i => i.Build(node));
        }

    }

}
