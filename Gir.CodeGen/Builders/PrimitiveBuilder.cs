using System;
using System.Collections.Generic;

using Gir.Model;

using Microsoft.CodeAnalysis;

namespace Gir.CodeGen.Builders
{

    /// <summary>
    /// Builds the syntax for class elements.
    /// </summary>
    class PrimitiveBuilder : SyntaxNodeBuilderBase<Primitive>
    {

        protected override IEnumerable<SyntaxNode> Build(IContext context, Primitive primitive)
        {
            yield return BuildPrimitive(context, primitive);
        }

        SyntaxNode BuildPrimitive(IContext context, Primitive primitive)
        {
            var attribute = BuildPrimitiveAttribute(context, primitive);
            if (attribute == null)
                return null;

            switch (attribute)
            {
                case Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax a:
                    return a.WithTarget(Microsoft.CodeAnalysis.CSharp.SyntaxFactory.AttributeTargetSpecifier(Microsoft.CodeAnalysis.CSharp.SyntaxFactory.Token(Microsoft.CodeAnalysis.CSharp.SyntaxKind.AssemblyKeyword)));
                case Microsoft.CodeAnalysis.VisualBasic.Syntax.AttributeListSyntax b:
                default:
                    throw new InvalidOperationException("Unknown language.");
            }
        }

        SyntaxNode BuildPrimitiveAttribute(IContext context, Primitive symbol)
        {

            return context.Syntax.Attribute(
                typeof(PrimitiveAttribute).FullName,
                BuildAttributeArguments(context, symbol));
        }

        IEnumerable<SyntaxNode> BuildAttributeArguments(IContext context, Primitive primitive)
        {
            var typeName = new GirTypeName(context.CurrentNamespace, primitive.Name);
            var clrTypeInfo = context.ResolveClrTypeInfo(typeName);
            if (clrTypeInfo == null)
                yield break;

            yield return BuildAttributeArgument(context, nameof(PrimitiveAttribute.Name), typeName.ToString());
            yield return BuildAttributeArgument(context, nameof(PrimitiveAttribute.CType), primitive.CType);
            yield return BuildAttributeArgument(context, nameof(PrimitiveAttribute.GLibTypeName), primitive.GLibTypeName);

            // primitive is associated with a specific CLR type
            if (clrTypeInfo.ClrTypeName != null)
                yield return context.Syntax.AttributeArgument(
                    nameof(PrimitiveAttribute.ClrType),
                    context.Syntax.TypeOfExpression(context.Syntax.DottedName(clrTypeInfo.ClrTypeName)));

            // primitive is to be marshaled with a specific marshaler
            if (clrTypeInfo.ClrMarshalerTypeName != null)
                yield return context.Syntax.AttributeArgument(
                    nameof(PrimitiveAttribute.ClrMarshalerType),
                    context.Syntax.TypeOfExpression(context.Syntax.DottedName(clrTypeInfo.ClrMarshalerTypeName)));
        }

        SyntaxNode BuildAttributeArgument(IContext context, string name, object value)
        {
            return context.Syntax.AttributeArgument(name, context.Syntax.LiteralExpression(value));
        }

    }

}
