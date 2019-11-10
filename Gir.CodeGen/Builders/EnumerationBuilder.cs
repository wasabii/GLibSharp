
using System.Collections.Generic;

using Gir.Model;

using Microsoft.CodeAnalysis;

namespace Gir.CodeGen
{

    /// <summary>
    /// Builds the syntax for class elements.
    /// </summary>
    class EnumerationBuilder : FlagBuilder<Enumeration>
    {

        protected override IEnumerable<SyntaxNode> BuildAttributes(IContext context, Enumeration enumeration)
        {
            foreach (var attr in base.BuildAttributes(context, enumeration))
                yield return attr;

            yield return BuildEnumerationAttribute(context, enumeration);
        }

        SyntaxNode BuildEnumerationAttribute(IContext context, Enumeration enumeration)
        {
            return context.Syntax.Attribute(typeof(EnumerationAttribute).FullName,
                context.Syntax.AttributeArgument(nameof(EnumerationAttribute.Name), context.Syntax.LiteralExpression(enumeration.Name)),
                context.Syntax.AttributeArgument(nameof(EnumerationAttribute.CType), context.Syntax.LiteralExpression(enumeration.CType)),
                context.Syntax.AttributeArgument(nameof(EnumerationAttribute.GLibGetType), context.Syntax.LiteralExpression(enumeration.GLibGetType)),
                context.Syntax.AttributeArgument(nameof(EnumerationAttribute.GLibTypeName), context.Syntax.LiteralExpression(enumeration.GLibTypeName)));
        }

    }

}
