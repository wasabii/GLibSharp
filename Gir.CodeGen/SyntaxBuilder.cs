using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace Gir.CodeGen
{

    /// <summary>
    /// Provides an entry point to generate .NET code from a set of GIR sources.
    /// </summary>
    class SyntaxBuilder : ISyntaxBuilder
    {

        /// <summary>
        /// Provides a context object for this builder.
        /// </summary>
        class SymbolBuilderContext : IContext
        {

            readonly ISymbolResolver resolver;
            readonly SyntaxGenerator syntax;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="resolver"></param>
            /// <param name="syntax"></param>
            public SymbolBuilderContext(ISymbolResolver resolver, SyntaxGenerator syntax)
            {
                this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
                this.syntax = syntax ?? throw new ArgumentNullException(nameof(syntax));
            }

            public ISymbolResolver Resolver => resolver;

            public SyntaxGenerator Syntax => syntax;

        }

        readonly SyntaxGenerator syntax;
        readonly IEnumerable<ISyntaxNodeBuilder> builders;
        readonly List<ISymbolSource> symbols = new List<ISymbolSource>();
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
        public ISyntaxBuilder AddSymbols(ISymbolSource source)
        {
            symbols.Add(source);
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
            return syntax.CompilationUnit(namespaces.Select(i => BuildNamespace(i)));
        }

        /// <summary>
        /// Initiates a build for the given namespace.
        /// </summary>
        /// <param name="ns"></param>
        /// <returns></returns>
        SyntaxNode BuildNamespace(string ns)
        {
            // provides recursive access to the symbols
            var context = new SymbolBuilderContext(new SymbolResolver(symbols), syntax);

            // initiate build of all available symbols in the namespace
            // pass results back through builders for adjustment
            return new SymbolResolver(symbols).Resolve(ns)
                .SelectMany(symbol => builders
                    .SelectMany(builder => builder.Build(context, symbol))
                    .Select(node => builders.Aggregate(node, (n, b) => b.Adjust(context, symbol, n) ?? n)))
                .FirstOrDefault(i => i != null);
        }

    }

}
