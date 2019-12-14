using System;
using System.Collections.Generic;

using GObject.Introspection.Reflection;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace GObject.Introspection.CodeGen
{

    /// <summary>
    /// Passes some contextual information down to each syntax node builder.
    /// </summary>
    public class SyntaxBuilderContext
    {

        readonly SyntaxGenerator syntax;
        readonly SyntaxBuilder builder;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="syntax"></param>
        /// <param name="builder"></param>
        public SyntaxBuilderContext(SyntaxGenerator syntax, SyntaxBuilder builder)
        {
            this.syntax = syntax ?? throw new ArgumentNullException(nameof(syntax));
            this.builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        /// <summary>
        /// Gets a reference to the syntax generator.
        /// </summary>
        public SyntaxGenerator Syntax => syntax;

        /// <summary>
        /// Builds the specified node.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public IEnumerable<SyntaxNode> Build(IIntrospectionNode node) => builder.BuildNode(node);

    }

}
