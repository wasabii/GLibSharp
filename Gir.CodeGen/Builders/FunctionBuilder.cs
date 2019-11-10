using System.Collections.Generic;

using Gir.Model;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace Gir.CodeGen.Builders
{

    /// <summary>
    /// Builds the syntax for method elements.
    /// </summary>
    class FunctionBuilder : CallableWithSignatureBuilderBase<Function>
    {

        protected override SyntaxNode BuildCallable(IContext context, Function function)
        {
            return BuildMethod(context, function);
        }

        protected virtual SyntaxNode BuildMethod(IContext context, Function function) =>
            context.Syntax.AddAttributes(
                context.Syntax.MethodDeclaration(
                    GetName(context, function),
                    BuildParameters(context, function),
                    BuildTypeParameters(context, function),
                    BuildReturnType(context, function),
                    GetAccessibility(context, function),
                    GetModifiers(context, function),
                    BuildStatements(context, function)),
                BuildAttributes(context, function))
            .NormalizeWhitespace();

        protected override DeclarationModifiers GetModifiers(IContext context, Function function)
        {
            return base.GetModifiers(context, function) | DeclarationModifiers.Static;
        }

        protected override IEnumerable<SyntaxNode> BuildAttributes(IContext context, Function function)
        {
            foreach (var attr in base.BuildAttributes(context, function))
                yield return attr;

            yield return BuildFunctionAttribute(context, function);
        }

        SyntaxNode BuildFunctionAttribute(IContext context, Function function)
        {
            return context.Syntax.Attribute(typeof(FunctionAttribute).FullName,
                context.Syntax.AttributeArgument(nameof(FunctionAttribute.Name), context.Syntax.LiteralExpression(function.Name)),
                context.Syntax.AttributeArgument(nameof(FunctionAttribute.CIdentifier), context.Syntax.LiteralExpression(function.CIdentifier)));
        }

    }

}
