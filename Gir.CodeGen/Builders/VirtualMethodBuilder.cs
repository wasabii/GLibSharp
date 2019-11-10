using System;
using System.Collections.Generic;

using Gir.CodeGen.Builders;
using Gir.Model;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace Gir.CodeGen
{

    /// <summary>
    /// Builds the syntax for method elements.
    /// </summary>
    class VirtualMethodBuilder : CallableWithSignatureBuilderBase<VirtualMethod>
    {

        protected override SyntaxNode BuildCallable(IContext context, VirtualMethod method)
        {
            return BuildMethod(context, method);
        }

        protected virtual SyntaxNode BuildMethod(IContext context, VirtualMethod method) =>
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

        protected override DeclarationModifiers GetModifiers(IContext context, VirtualMethod method)
        {
            var sigOnly = context.Annotation<CallableBuilderOptions>()?.SignatureOnly == true;
            if (sigOnly == true)
                return base.GetModifiers(context, method);
            else
                return base.GetModifiers(context, method) | DeclarationModifiers.Virtual;
        }

        IEnumerable<SyntaxNode> BuildAttributes(IContext context, VirtualMethod method)
        {
            yield return context.Syntax.Attribute(
                typeof(VirtualMethodAttribute).FullName,
                context.Syntax.AttributeArgument(context.Syntax.LiteralExpression(method.Name)));

            if (method.Info.Deprecated == true)
                yield return context.Syntax.Attribute(typeof(ObsoleteAttribute).FullName);
        }

    }

}
