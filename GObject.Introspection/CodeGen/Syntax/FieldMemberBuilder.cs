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
    class FieldMemberBuilder : SyntaxMemberBuilderBase<FieldMember>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="member"></param>
        public FieldMemberBuilder(ModuleContext context, FieldMember member) :
            base(context, member)
        {

        }

        public override IEnumerable<SyntaxNode> Build()
        {
            yield return BuildField(Member);
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
            return Syntax.TypeSymbol(field.FieldType);
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
