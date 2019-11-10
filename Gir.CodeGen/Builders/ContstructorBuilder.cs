using System;
using System.Collections.Generic;
using System.Linq;

using Gir.Model;

using Microsoft.CodeAnalysis;

namespace Gir.CodeGen.Builders
{

    /// <summary>
    /// Builds the syntax for method elements.
    /// </summary>
    class ConstructorBuilder : CallableWithSignatureBuilderBase<Constructor>
    {

        protected override SyntaxNode BuildCallable(IContext context, Constructor ctor)
        {
            return BuildConstructor(context, ctor);
        }

        SyntaxNode BuildConstructor(IContext context, Constructor ctor) =>
            context.Syntax.AddAttributes(
                context.Syntax.ConstructorDeclaration(
                    GetContainingTypeName(context, ctor),
                    BuildParameters(context, ctor),
                    GetAccessibility(context, ctor),
                    GetModifiers(context, ctor),
                    BuildBaseArguments(context, ctor),
                    BuildStatements(context, ctor)),
                BuildAttributes(context, ctor))
            .NormalizeWhitespace();

        /// <summary>
        /// Gets the name of the parent class or struct.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ctor"></param>
        /// <returns></returns>
        string GetContainingTypeName(IContext context, Constructor ctor)
        {
            var parentClrTypeName = context.Annotation<CallableBuilderOptions>()?.ClrTypeName;
            if (parentClrTypeName == null)
                throw new InvalidOperationException("Missing callable builder options.");

            return parentClrTypeName.Value.Name;
        }

        IEnumerable<SyntaxNode> BuildBaseArguments(IContext context, Constructor ctor)
        {
            yield break;
        }

        protected override IEnumerable<SyntaxNode> BuildAttributes(IContext context, Constructor ctor)
        {
            foreach (var attr in base.BuildAttributes(context, ctor))
                yield return attr;

            yield return BuildConstructorAttribute(context, ctor);
        }

        SyntaxNode BuildConstructorAttribute(IContext context, Constructor ctor)
        {
            return context.Syntax.Attribute(
                typeof(ConstructorAttribute).FullName,
                context.Syntax.AttributeArgument(context.Syntax.LiteralExpression(ctor.Name)));
        }

    }

}
