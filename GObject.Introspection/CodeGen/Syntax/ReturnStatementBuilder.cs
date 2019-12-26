using System.Collections.Generic;
using System.Linq;

using GObject.Introspection.CodeGen.Model.Expressions;

using Microsoft.CodeAnalysis;

namespace GObject.Introspection.CodeGen.Syntax
{

    class ReturnStatementBuilder : SyntaxStatementBuilderBase<ReturnStatement>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="statement"></param>
        public ReturnStatementBuilder(ModuleContext context, ReturnStatement statement) :
            base(context, statement)
        {

        }

        public override SyntaxNode Build()
        {
            return Syntax.ReturnStatement(Context.Build(Statement.Expression));
        }

    }

}