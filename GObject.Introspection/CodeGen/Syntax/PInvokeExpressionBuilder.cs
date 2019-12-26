using System.Collections.Generic;
using System.Linq;

using GObject.Introspection.CodeGen.Model;
using GObject.Introspection.CodeGen.Model.Expressions;

using Microsoft.CodeAnalysis;

namespace GObject.Introspection.CodeGen.Syntax
{

    class PInvokeExpressionBuilder : SyntaxExpressionBuilderBase<PInvokeExpression>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="expression"></param>
        public PInvokeExpressionBuilder(ModuleContext context, PInvokeExpression expression) :
            base(context, expression)
        {

        }

        public override SyntaxNode Build()
        {
            return Syntax.InvocationExpression(
                Syntax.DottedName("__" + Expression.Function.EntryPoint),
                Expression.Parameters.Zip(Expression.Function.Arguments, (e, a) => BuildArgument(a, e)));
        }

        SyntaxNode BuildArgument(Argument argument, Expression value)
        {
            return Context.Build(value);
        }

    }

}