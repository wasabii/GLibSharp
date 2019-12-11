using System;
using System.Collections.Generic;

using GObject.Introspection.Model;

using Microsoft.CodeAnalysis;

namespace GObject.Introspection.CodeGen
{

    class BitFieldBuilder : FlagBuilder<BitField>
    {

        protected override IEnumerable<SyntaxNode> BuildAttributes(IContext context, BitField flag)
        {
            foreach (var attr in base.BuildAttributes(context, flag))
                yield return attr;

            yield return context.Syntax.Attribute(typeof(FlagsAttribute).FullName);
            yield return BuildBitFieldAttribute(context, flag);
        }

        SyntaxNode BuildBitFieldAttribute(IContext context, BitField flag)
        {
            return context.Syntax.Attribute(
                typeof(BitFieldAttribute).FullName,
                BuildBitFieldAttributeArguments(context, flag));
        }

        IEnumerable<SyntaxNode> BuildBitFieldAttributeArguments(IContext context, BitField flag)
        {
            yield return context.Syntax.AttributeArgument(context.Syntax.LiteralExpression(flag.Name));

            if (flag.CType != null)
                yield return context.Syntax.AttributeArgument(nameof(BitFieldAttribute.CType), context.Syntax.LiteralExpression(flag.CType));

            if (flag.GLibGetType != null)
                yield return context.Syntax.AttributeArgument(nameof(BitFieldAttribute.GLibGetType), context.Syntax.LiteralExpression(flag.GLibGetType));

            if (flag.GLibTypeName != null)
                yield return context.Syntax.AttributeArgument(nameof(BitFieldAttribute.GLibTypeName), context.Syntax.LiteralExpression(flag.GLibTypeName));
        }

    }

}
