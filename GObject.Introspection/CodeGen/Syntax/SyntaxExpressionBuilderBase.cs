using System;
using System.Collections.Generic;

using GObject.Introspection.CodeGen.Model;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace GObject.Introspection.CodeGen.Syntax
{

    /// <summary>
    /// Implements the base class for building syntax nodes from expressions.
    /// </summary>
    /// <typeparam name="TExpression"></typeparam>
    abstract partial class SyntaxExpressionBuilderBase<TExpression> : SyntaxExpressionBuilderBase
        where TExpression : Expression
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="expression"></param>
        public SyntaxExpressionBuilderBase(ModuleContext context, TExpression expression) :
            base(context, expression)
        {

        }

        /// <summary>
        /// Gets the member to be built.
        /// </summary>
        protected new TExpression Expression => (TExpression)base.Expression;

    }

    /// <summary>
    /// Implements the base class for building syntax nodes from expressions.
    /// </summary>
    abstract partial class SyntaxExpressionBuilderBase
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="expression"></param>
        public SyntaxExpressionBuilderBase(ModuleContext context, Expression expression)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Expression = expression ?? throw new ArgumentNullException(nameof(expression));
        }

        /// <summary>
        /// Gets the generator used to create syntax nodes.
        /// </summary>
        protected ModuleContext Context { get; }

        /// <summary>
        /// Gets the member to be built.
        /// </summary>
        protected Expression Expression { get; }

        /// <summary>
        /// Gets a reference to the syntax generator.
        /// </summary>
        protected SyntaxGenerator Syntax => Context.Syntax;

        /// <summary>
        /// Builds the syntax for the member.
        /// </summary>
        /// <returns></returns>
        public abstract SyntaxNode Build();

    }

}
