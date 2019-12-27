using System.Linq;

using GObject.Introspection.CodeGen.Model;
using GObject.Introspection.CodeGen.Model.Expressions;

using Microsoft.CodeAnalysis;

namespace GObject.Introspection.CodeGen.Syntax
{

    class PInvokeStatementBuilder : SyntaxStatementBuilderBase<PInvokeStatement>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="statement"></param>
        public PInvokeStatementBuilder(ModuleContext context, PInvokeStatement statement) :
            base(context, statement)
        {

        }

        public override SyntaxNode Build()
        {
            return Syntax.InvocationExpression(
                Syntax.DottedName("__" + Statement.Function.EntryPoint),
                Statement.Arguments.Zip(Statement.Function.Parameters, (e, a) => BuildArgument(a, e)));
        }

        SyntaxNode BuildArgument(Parameter argument, Expression value)
        {
            return Context.Build(value);
        }

    }

}