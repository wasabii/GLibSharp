using System;
using System.Collections.Generic;
using System.Linq;

using GObject.Introspection.CodeGen.Model;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace GObject.Introspection.CodeGen.Syntax
{

    /// <summary>
    /// Builds the syntax for class elements.
    /// </summary>
    class EnumTypeBuilder : SyntaxTypeBuilderBase
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public EnumTypeBuilder(Context context) :
            base(context)
        {

        }

        public IEnumerable<SyntaxNode> Build(EnumType type)
        {
            yield return BuildEnum(type);
        }

        protected virtual SyntaxNode BuildEnum(EnumType type) =>
            Syntax.AddAttributes(
                Syntax.EnumDeclaration(
                    GetName(type),
                    GetAccessibility(type),
                    GetModifiers(type),
                    BuildMembers(type)),
                GetAttributes(type));

        string GetName(EnumType type)
        {
            return type.Name;
        }

        Accessibility GetAccessibility(EnumType type)
        {
            switch (type.Visibility)
            {
                case Visibility.Public:
                    return Accessibility.Public;
                case Visibility.Private:
                    return Accessibility.Private;
                case Visibility.Internal:
                    return Accessibility.Internal;
                default:
                    throw new InvalidOperationException();
            }
        }

        DeclarationModifiers GetModifiers(EnumType type)
        {
            var modifiers = DeclarationModifiers.Partial;

            if (type.Modifiers.HasFlag(Modifier.Static))
                modifiers |= DeclarationModifiers.Static;

            if (type.Modifiers.HasFlag(Modifier.Abstract))
                modifiers |= DeclarationModifiers.Abstract;

            return modifiers;
        }

        IEnumerable<SyntaxNode> BuildMembers(EnumType type)
        {
            return type.Members.OfType<EnumMember>().SelectMany(i => BuildMember(type, i));
        }

        IEnumerable<SyntaxNode> BuildMember(EnumType type, EnumMember member)
        {
            yield return Syntax.EnumMember(member.Name, ConvertValue(member.Value));
        }

        SyntaxNode ConvertValue(int value)
        {
            return Syntax.LiteralExpression(value);
        }

        IEnumerable<SyntaxNode> GetAttributes(EnumType type)
        {
            return type.Attributes.Select(i => GetAttribute(type, i)).Where(i => i != null);
        }

        SyntaxNode GetAttribute(EnumType type, Attribute attribute)
        {
            return Syntax.Attribute(attribute.GetType().FullName);
        }

    }

}
