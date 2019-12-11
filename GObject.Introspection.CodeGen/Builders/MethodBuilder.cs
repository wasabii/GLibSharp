using System;
using System.Collections.Generic;
using GObject.Introspection.CodeGen.Builders;
using GObject.Introspection.Model;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace GObject.Introspection.CodeGen
{

    /// <summary>
    /// Builds the syntax for method elements.
    /// </summary>
    class MethodBuilder : CallableWithSignatureBuilderBase<Method>
    {

        protected override SyntaxNode BuildCallable(IContext context, Method method)
        {
            return BuildMethod(context, method);
        }

        protected virtual SyntaxNode BuildMethod(IContext context, Method method) =>
            context.Syntax.AddAttributes(
                context.Syntax.MethodDeclaration(
                    GetName(context, method),
                    BuildParameters(context, method),
                    BuildTypeParameters(context, method),
                    BuildReturnType(context, method),
                    GetAccessibility(context, method),
                    GetModifiers(context, method),
                    BuildStatements(context, method)),
                BuildAttributes(context, method))
            .NormalizeWhitespace();

        protected override DeclarationModifiers GetModifiers(IContext context, Method symbol)
        {
            return base.GetModifiers(context, symbol);
        }

        protected override IEnumerable<SyntaxNode> BuildAttributes(IContext context, Method method)
        {
            foreach (var attr in base.BuildAttributes(context, method))
                yield return attr;

            yield return BuildMethodAttribute(context, method);
        }

        SyntaxNode BuildMethodAttribute(IContext context, Method method)
        {
            return context.Syntax.Attribute(
                typeof(MethodAttribute).FullName,
                context.Syntax.AttributeArgument(context.Syntax.LiteralExpression(method.Name)));
        }

    }

}
