using System.Collections.Generic;

using GObject.Introspection.CodeGen.Model;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace GObject.Introspection.CodeGen.Syntax
{

    /// <summary>
    /// Builds the syntax for property elements.
    /// </summary>
    class PropertyMemberBuilder : SyntaxMemberBuilderBase
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public PropertyMemberBuilder(Context context) :
            base(context)
        {

        }

        public IEnumerable<SyntaxNode> Build(PropertyMember property)
        {
            yield return BuildProperty(property);
        }

        SyntaxNode BuildProperty(PropertyMember property) =>
            Syntax.AddAttributes(
                Syntax.PropertyDeclaration(
                    GetName(property),
                    BuildType(property),
                    GetAccessibility(property),
                    GetModifiers(property),
                    BuildGetAccessorStatements(property),
                    BuildSetAccessorStatements(property)),
                BuildAttributes(property))
            .NormalizeWhitespace();

        string GetName(PropertyMember property)
        {
            return property.Name;
        }

        /// <summary>
        /// Resolves the specified type of a property.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        SyntaxNode BuildType(PropertyMember property)
        {
            return Syntax.DottedName(property.PropertyType.Type.Name);
        }

        Accessibility GetAccessibility(PropertyMember property)
        {
            return Accessibility.Public;
        }

        DeclarationModifiers GetModifiers(PropertyMember property)
        {
            return DeclarationModifiers.Partial;
        }

        IEnumerable<SyntaxNode> BuildGetAccessorStatements(PropertyMember property)
        {
            if (property.GetGetterInvokable() is IntrospectionInvokable invokable)
                yield return Syntax.ReturnStatement(
                    Syntax.InvocationExpression(
                        Syntax.IdentifierName("get_property"),
                        Syntax.ThisExpression(),
                        Syntax.LiteralExpression(property.Name)));
        }

        IEnumerable<SyntaxNode> BuildSetAccessorStatements(PropertyMember property)
        {
            if (property.GetSetterInvokable() is IntrospectionInvokable invokable)
                yield return Syntax.InvocationExpression(
                    Syntax.IdentifierName("set_property"),
                    Syntax.ThisExpression(),
                    Syntax.LiteralExpression(property.Name),
                    Syntax.IdentifierName("value"));
        }

        protected IEnumerable<SyntaxNode> BuildAttributes(PropertyMember property)
        {
            yield return BuildPropertyAttribute(property);
        }

        SyntaxNode BuildPropertyAttribute(PropertyMember property)
        {
            return Syntax.Attribute(
                typeof(PropertyAttribute).FullName,
                Syntax.AttributeArgument(Syntax.LiteralExpression(property.Name)));
        }

    }

}
