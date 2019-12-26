using System;

using GObject.Introspection.CodeGen.Model;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace GObject.Introspection.CodeGen.Syntax
{

    /// <summary>
    /// Implements the base class for building syntax nodes from expressions.
    /// </summary>
    /// <typeparam name="TStatement"></typeparam>
    abstract partial class SyntaxStatementBuilderBase<TStatement> : SyntaxStatementBuilderBase
        where TStatement : Statement
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="statement"></param>
        public SyntaxStatementBuilderBase(ModuleContext context, TStatement statement) :
            base(context, statement)
        {

        }

        /// <summary>
        /// Gets the member to be built.
        /// </summary>
        protected new TStatement Statement => (TStatement)base.Statement;

    }

    /// <summary>
    /// Implements the base class for building syntax nodes from expressions.
    /// </summary>
    abstract partial class SyntaxStatementBuilderBase 
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="statement"></param>
        public SyntaxStatementBuilderBase(ModuleContext context, Statement statement)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Statement = statement ?? throw new ArgumentNullException(nameof(statement));
        }

        /// <summary>
        /// Gets the generator used to create syntax nodes.
        /// </summary>
        protected ModuleContext Context { get; }

        /// <summary>
        /// Gets the statement to be built.
        /// </summary>
        protected Statement Statement { get; }

        /// <summary>
        /// Gets a reference to the syntax generator.
        /// </summary>
        protected SyntaxGenerator Syntax => Context.Syntax;

        /// <summary>
        /// Builds the syntax for the statement.
        /// </summary>
        /// <returns></returns>
        public abstract SyntaxNode Build();

    }

}
