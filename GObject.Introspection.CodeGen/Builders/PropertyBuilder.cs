using System.Collections.Generic;

using GObject.Introspection.CodeGen.Builders;
using GObject.Introspection.Model;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace GObject.Introspection.CodeGen
{

    /// <summary>
    /// Builds the syntax for property elements.
    /// </summary>
    class PropertyBuilder : SyntaxNodeBuilderBase<Property>
    {

        protected override IEnumerable<SyntaxNode> Build(IContext context, Property property)
        {
            yield return BuildProperty(context, property);
        }

        SyntaxNode BuildProperty(IContext context, Property property) =>
            context.Syntax.AddAttributes(
                context.Syntax.PropertyDeclaration(
                    GetName(context, property),
                    BuildType(context, property),
                    GetAccessibility(context, property),
                    GetModifiers(context, property),
                    BuildGetAccessorStatements(context, property),
                    BuildSetAccessorStatements(context, property)),
                BuildAttributes(context, property))
            .NormalizeWhitespace();

        string GetName(IContext context, Property property)
        {
            return property.Name.ToPascalCase();
        }

        /// <summary>
        /// Resolves the specified type of a property.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        SyntaxNode BuildType(IContext context, Property property)
        {
            return GetType(context, property, property.Type);
        }

        /// <summary>
        /// Resolves the specified type in relation to a property.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="property"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        SyntaxNode GetType(IContext context, Property property, AnyType type)
        {
            var typeSpec = BuilderUtil.GetTypeSpec(context, type);
            if (typeSpec == null)
                throw new GirException("Unable to determine type specification.");

            return typeSpec.GetClrTypeExpression(context.Syntax);
        }

        Accessibility GetAccessibility(IContext context, Property property)
        {
            return Accessibility.Public;
        }

        DeclarationModifiers GetModifiers(IContext context, Property property)
        {
            return DeclarationModifiers.Partial;
        }

        IEnumerable<SyntaxNode> BuildGetAccessorStatements(IContext context, Property property)
        {
            if (property.Readable != false)
                yield return context.Syntax.ReturnStatement(
                    context.Syntax.InvocationExpression(
                        context.Syntax.IdentifierName("get_property"),
                        context.Syntax.ThisExpression(),
                        context.Syntax.LiteralExpression(property.Name)));
        }

        IEnumerable<SyntaxNode> BuildSetAccessorStatements(IContext context, Property property)
        {
            if (property.Writable != false)
                yield return context.Syntax.InvocationExpression(
                    context.Syntax.IdentifierName("set_property"),
                    context.Syntax.ThisExpression(),
                    context.Syntax.LiteralExpression(property.Name),
                    context.Syntax.IdentifierName("value"));
        }

        protected override IEnumerable<SyntaxNode> BuildAttributes(IContext context, Property property)
        {
            foreach (var attr in base.BuildAttributes(context, property))
                yield return attr;

            yield return BuildPropertyAttribute(context, property);
        }

        SyntaxNode BuildPropertyAttribute(IContext context, Property property)
        {
            return context.Syntax.Attribute(
                typeof(PropertyAttribute).FullName,
                context.Syntax.AttributeArgument(context.Syntax.LiteralExpression(property.Name)));
        }

    }

}
