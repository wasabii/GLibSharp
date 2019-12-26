using System.Linq;

using GObject.Introspection.CodeGen.Model.Expressions;

using Microsoft.CodeAnalysis;

namespace GObject.Introspection.CodeGen.Syntax
{

    class InvokeExpressionBuilder : SyntaxExpressionBuilderBase<InvokeExpression>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="expression"></param>
        public InvokeExpressionBuilder(ModuleContext context, InvokeExpression expression) :
            base(context, expression)
        {

        }

        public override SyntaxNode Build()
        {
            if (Expression.Instance == null)
                return Syntax.InvocationExpression(
                    Syntax.MemberAccessExpression(Syntax.TypeSymbol(Expression.MethodType), Expression.MethodName),
                    Expression.Parameters.Select(i => Context.Build(i)));
            else
                return Syntax.InvocationExpression(
                    Syntax.MemberAccessExpression(Context.Build(Expression.Instance), Expression.MethodName),
                    Expression.Parameters.Select(i => Context.Build(i)));
        }

    }

}