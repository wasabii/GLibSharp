using System.Collections.Generic;

using GObject.Introspection.Reflection;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace GObject.Introspection.CodeGen
{

    /// <summary>
    /// Builds the syntax for field members.
    /// </summary>
    class FieldBuilder : SyntaxNodeBuilderBase<FieldMember>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public FieldBuilder(SyntaxBuilderContext context) :
            base(context)
        {

        }

        protected override IEnumerable<SyntaxNode> Build(FieldMember field)
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
            return Syntax.DottedName(field.FieldType.Name);
        }

        Accessibility GetAccessibility(FieldMember field)
        {
            return Accessibility.Public;
        }

        DeclarationModifiers GetModifiers(FieldMember field)
        {
            return DeclarationModifiers.Partial;
        }

        SyntaxNode GetInitializer(FieldMember field)
        {
            return null;
        }

        protected IEnumerable<SyntaxNode> BuildAttributes(FieldMember field)
        {
            yield return BuildFieldAttribute(field);
        }

        SyntaxNode BuildFieldAttribute(FieldMember field)
        {
            return null;
        }

    }

}
