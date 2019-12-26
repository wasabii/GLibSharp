using System;
using System.Collections.Generic;

using GObject.Introspection.CodeGen.Model;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace GObject.Introspection.CodeGen.Syntax
{

    /// <summary>
    /// Builds the syntax for field members.
    /// </summary>
    class FieldMemberBuilder : SyntaxMemberBuilderBase
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public FieldMemberBuilder(Context context) :
            base(context)
        {

        }

        public IEnumerable<SyntaxNode> Build(FieldMember field)
        {
            yield return BuildField(field);
        }

        SyntaxNode BuildField(FieldMember field) =>
            Syntax.AddAttributes(
                Syntax.FieldDeclaration(
                    GetName(field),
                    BuildType(field),
                    GetAccessibility(field),
                    GetModifiers(field),
                    GetInitializer(field)),
                BuildAttributes(field))
            .NormalizeWhitespace();

        string GetName(FieldMember field)
        {
            return field.Name;
        }

        /// <summary>
        /// Resolves the specified type of a field.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        SyntaxNode BuildType(FieldMember field)
        {
            return Syntax.DottedName(field.FieldType.Type.Name);
        }

        Accessibility GetAccessibility(FieldMember field)
        {
            switch (field.Visibility)
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

        DeclarationModifiers GetModifiers(FieldMember field)
        {
            var modifiers = DeclarationModifiers.None;

            if (field.Modifiers.HasFlag(MemberModifier.Static))
                modifiers |= DeclarationModifiers.Static;

            if (field.Modifiers.HasFlag(MemberModifier.Abstract))
                modifiers |= DeclarationModifiers.Abstract;

            return modifiers;
        }

        SyntaxNode GetInitializer(FieldMember field)
        {
            return null;
        }

        IEnumerable<SyntaxNode> BuildAttributes(FieldMember field)
        {
            yield break;
        }

    }

}
