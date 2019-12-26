using System;
using System.Collections.Generic;
using System.Linq;

using GObject.Introspection.CodeGen.Model;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace GObject.Introspection.CodeGen.Syntax
{

    /// <summary>
    /// Provides an entry point to generate .NET code from a set of GIR sources.
    /// </summary>
    public class SyntaxModuleBuilder
    {

        readonly ModuleLibrary library;
        readonly SyntaxGenerator syntax;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="library"></param>
        /// <param name="syntax"></param>
        public SyntaxModuleBuilder(ModuleLibrary library, SyntaxGenerator syntax)
        {
            this.library = library ?? throw new ArgumentNullException(nameof(library));
            this.syntax = syntax ?? throw new ArgumentNullException(nameof(syntax));
        }

        /// <summary>
        /// Exports the code required to generate the specified module.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public SyntaxNode BuildModule(string name, string version)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));
            if (version is null)
                throw new ArgumentNullException(nameof(version));

            var module = library.ResolveModule(name, version);
            if (module == null)
                throw new SyntaxBuilderException("Unable to resolve specified namespace.");

            // build types in each emitted namespace
            var s = BuildModule(module);
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
        IEnumerable<SyntaxNode> BuildModule(Module module)
        {
            yield return syntax.NamespaceDeclaration(module.Name, module.Types.SelectMany(i => BuildType(i)));
        }

        /// <summary>
        /// Initiates a build for the given node.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal IEnumerable<SyntaxNode> BuildType(Dynamic.Type type)
        {
            return new Context(syntax, this).Build(type);
        }

    }

}
