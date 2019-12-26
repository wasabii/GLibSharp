
using GObject.Introspection.CodeGen.Model.Expressions;

using Microsoft.CodeAnalysis;

namespace GObject.Introspection.CodeGen.Syntax
{

    class ExpressionStatementBuilder : SyntaxStatementBuilderBase<ExpressionStatement>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="statement"></param>
        public ExpressionStatementBuilder(ModuleContext context, ExpressionStatement statement) :
            base(context, statement)
        {

        }

        public override SyntaxNode Build()
        {
            return Syntax.ExpressionStatement(Context.Build(Statement.Expression));
        }

    }

}