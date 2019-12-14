using System;
using System.Collections.Generic;
using System.Linq;

using GObject.Introspection.Reflection;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace GObject.Introspection.CodeGen
{

    /// <summary>
    /// Builds the syntax for class elements.
    /// </summary>
    abstract class EnumBuilder : SyntaxNodeBuilderBase<EnumType>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public EnumBuilder(SyntaxBuilderContext context) :
            base(context)
        {

        }

        protected override IEnumerable<SyntaxNode> Build(EnumType type)
        {
            yield return BuildEnum(type);
        }

        protected virtual SyntaxNode BuildEnum(EnumType type) =>
            Syntax.EnumDeclaration(
                GetName(type),
                GetAccessibility(type),
                GetModifiers(type),
                BuildMembers(type));

        string GetName(EnumType type)
        {
            return type.Name;
        }

        Accessibility GetAccessibility(EnumType type)
        {
            return Accessibility.Public;
        }

        DeclarationModifiers GetModifiers(EnumType type)
        {
            return DeclarationModifiers.Partial;
        }

        IEnumerable<SyntaxNode> BuildMembers(EnumType type)
        {
            return type.Members.OfType<EnumerationMember>().SelectMany(i => BuildMember(type, i));
        }

        IEnumerable<SyntaxNode> BuildMember(EnumType type, EnumerationMember member)
        {
            yield return Syntax.EnumMember(member.Name, ConvertValue(member.Value));
        }

        SyntaxNode ConvertValue(string value)
        {
            if (long.TryParse(value, out var l))
                return Syntax.LiteralExpression((int)l);

            throw new InvalidOperationException();
        }

    }

}
