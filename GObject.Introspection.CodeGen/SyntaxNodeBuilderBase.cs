using System;
using System.Collections.Generic;

using GObject.Introspection.Reflection;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace GObject.Introspection.CodeGen
{

    /// <summary>
    /// Implements the base class for building the syntax for an introspection node.
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    abstract partial class SyntaxNodeBuilderBase<TNode> : SyntaxNodeBuilderBase
        where TNode : IIntrospectionNode
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public SyntaxNodeBuilderBase(SyntaxBuilderContext context) :
            base(context)
        {

        }

        /// <summary>
        /// Builds the syntax for the specified object.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected abstract IEnumerable<SyntaxNode> Build(TNode node);

        /// <summary>
        /// Builds the specified introspection node.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public override sealed IEnumerable<SyntaxNode> Build(IIntrospectionNode node)
        {
            return node is TNode n ? Build(n) : null;
        }

    }

    /// <summary>
    /// Implements the base class for building a syntax node.
    /// </summary>
    abstract partial class SyntaxNodeBuilderBase
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public SyntaxNodeBuilderBase(SyntaxBuilderContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Gets the generator used to create syntax nodes.
        /// </summary>
        protected SyntaxBuilderContext Context { get; }

        /// <summary>
        /// Gets a reference to the syntax generator.
        /// </summary>
        protected SyntaxGenerator Syntax => Context.Syntax;

        /// <summary>
        /// Attempts to build the specified node.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public abstract IEnumerable<SyntaxNode> Build(IIntrospectionNode node);

    }

}
