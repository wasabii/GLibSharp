using System;
using System.Collections.Generic;
using Gir.Model;

using Microsoft.CodeAnalysis;

namespace Gir.CodeGen
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
            return context.Syntax.Attribute(typeof(BitFieldAttribute).FullName,
                context.Syntax.AttributeArgument(nameof(BitFieldAttribute.Name), context.Syntax.LiteralExpression(flag.Name)),
                context.Syntax.AttributeArgument(nameof(BitFieldAttribute.CType), context.Syntax.LiteralExpression(flag.CType)),
                context.Syntax.AttributeArgument(nameof(BitFieldAttribute.GLibGetType), context.Syntax.LiteralExpression(flag.GLibGetType)),
                context.Syntax.AttributeArgument(nameof(BitFieldAttribute.GLibTypeName), context.Syntax.LiteralExpression(flag.GLibTypeName)));
        }

    }

}
