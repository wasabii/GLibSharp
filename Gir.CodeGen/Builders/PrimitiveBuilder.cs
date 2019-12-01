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
            var typeName = new TypeName(context.CurrentNamespace, primitive.Name);
            var typeInfo = context.ResolveTypeInfo(typeName);
            if (typeInfo == null)
                yield break;

            yield return context.Syntax.AttributeArgument(context.Syntax.LiteralExpression(typeName.ToString()));

            var typeSpec = new TypeSpec(typeInfo);
            var typeNode = typeSpec.GetClrTypeExpression(context.Syntax);

            // primitive is associated with a specific CLR type
            if (typeNode != null)
                yield return context.Syntax.AttributeArgument(context.Syntax.TypeOfExpression(typeNode));

            if (primitive.CType != null)
                yield return context.Syntax.AttributeArgument(
                    nameof(PrimitiveAttribute.CType),
                    context.Syntax.LiteralExpression(primitive.CType));

            if (primitive.GLibTypeName != null)
                yield return context.Syntax.AttributeArgument(
                    nameof(PrimitiveAttribute.GLibTypeName),
                    context.Syntax.LiteralExpression(primitive.GLibTypeName));

            // primitive is to be marshaled with a specific marshaler
            if (typeInfo.ClrMarshalerTypeExpression != null)
                yield return context.Syntax.AttributeArgument(
                    nameof(PrimitiveAttribute.ClrMarshalerType),
                    context.Syntax.TypeOfExpression(typeSpec.GetClrMarshalerTypeExpression(context.Syntax)));
        }

    }

}
