using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

using GObject.Introspection.CodeGen.Model;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace GObject.Introspection.CodeGen.Syntax
{

    /// <summary>
    /// Builds the syntax for structure types.
    /// </summary>
    class StructureTypeBuilder : SyntaxTypeBuilderBase
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public StructureTypeBuilder(Context context) :
            base(context)
        {

        }

        public IEnumerable<SyntaxNode> Build(StructureType structure)
        {
            yield return BuildStructure(structure);
        }

        SyntaxNode BuildStructure(StructureType structure) =>
            Syntax.AddAttributes(
                Syntax.StructDeclaration(
                    GetName(structure),
                    BuildTypeParameters(structure),
                    GetAccessibility(structure),
                    GetModifiers(structure),
                    BuildInterfaceTypes(structure),
                    BuildMembers(structure)),
                BuildAttributes(structure))
            .NormalizeWhitespace();

        string GetName(StructureType structure)
        {
            return structure.Name;
        }

        IEnumerable<string> BuildTypeParameters(StructureType structure)
        {
            yield break;
        }

        Accessibility GetAccessibility(StructureType structure)
        {
            switch (structure.Visibility)
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

        DeclarationModifiers GetModifiers(StructureType structure)
        {
            var m = DeclarationModifiers.Partial;

            if (structure.Modifiers.HasFlag(Modifier.Static))
                m |= DeclarationModifiers.Static;

            if (structure.Modifiers.HasFlag(Modifier.Abstract))
                m |= DeclarationModifiers.Abstract;

            return m;
        }

        IEnumerable<SyntaxNode> BuildInterfaceTypes(StructureType structure)
        {
            return structure.ImplementedInterfaces.Select(i => Syntax.DottedName(i.Name));
        }

        IEnumerable<SyntaxNode> BuildMembers(StructureType structure)
        {
            // build individual members
            foreach (var node in structure.Members.SelectMany(i => Context.Build(i)))
                yield return node;

            // hard coded members
            yield return BuildEqualsMethod(structure);
            yield return BuildTypedEqualsMethod(structure);
            yield return BuildGetHashCodeMethod(structure);
        }

        SyntaxNode BuildEqualsMethod(StructureType structure)
        {
            return Syntax.MethodDeclaration(
                nameof(System.Object.Equals),
                new[]
                {
                    Syntax.ParameterDeclaration(
                        "other",
                        Syntax.TypeExpression(SpecialType.System_Object))
                },
                null,
                Syntax.TypeExpression(SpecialType.System_Boolean),
                Accessibility.Public,
                DeclarationModifiers.Override,
                new[]
                {
                    Syntax.ReturnStatement(
                        Syntax.ConditionalExpression(
                            Syntax.LogicalNotExpression(
                                Syntax.IsTypeExpression(
                                    Syntax.IdentifierName("other"),
                                    Syntax.IdentifierName(structure.Name))),
                            Syntax.FalseLiteralExpression(),
                            Syntax.InvocationExpression(
                                Syntax.MemberAccessExpression(
                                    Syntax.ThisExpression(),
                                    Syntax.IdentifierName(nameof(System.Object.Equals))),
                                Syntax.CastExpression(
                                    Syntax.IdentifierName(structure.Name),
                                    Syntax.IdentifierName("other")))))
                });
        }

        SyntaxNode BuildTypedEqualsMethod(StructureType record)
        {
            return Syntax.MethodDeclaration(
                nameof(System.Object.Equals),
                new[]
                {
                    Syntax.ParameterDeclaration(
                        "other",
                        Syntax.IdentifierName(record.Name))
                },
                null,
                Syntax.TypeExpression(SpecialType.System_Boolean),
                Accessibility.Public,
                DeclarationModifiers.None,
                new[]
                {
                    Syntax.ReturnStatement(
                        Syntax.LogicalAndExpression(record.Members.OfType<FieldMember>().Select(i =>
                            Syntax.InvocationExpression(
                                Syntax.MemberAccessExpression(
                                    Syntax.MemberAccessExpression(
                                        Syntax.ThisExpression(),
                                        Syntax.IdentifierName(i.Name)),
                                    nameof(System.Object.Equals)),
                                Syntax.MemberAccessExpression(
                                    Syntax.IdentifierName("other"),
                                    Syntax.IdentifierName(i.Name))))))
                });
        }

        SyntaxNode BuildGetHashCodeMethod(StructureType record)
        {
            return Syntax.MethodDeclaration(
                nameof(System.Object.GetHashCode),
                null,
                null,
                Syntax.TypeExpression(SpecialType.System_Int32),
                Accessibility.Public,
                DeclarationModifiers.Override,
                new[]
                {
                    Syntax.ReturnStatement(
                        Syntax.ExclusiveOrExpression(
                            Syntax.InvocationExpression(
                                Syntax.MemberAccessExpression(
                                    Syntax.MemberAccessExpression(
                                        Syntax.InvocationExpression(
                                            Syntax.MemberAccessExpression(
                                                Syntax.ThisExpression(),
                                                nameof(System.Type.GetType))),
                                        nameof(System.Type.FullName)),
                                    nameof(System.Object.GetHashCode))),
                            Syntax.ExclusiveOrExpression(record.Members.OfType<FieldMember>().Select(i =>
                                Syntax.InvocationExpression(
                                    Syntax.MemberAccessExpression(
                                        Syntax.MemberAccessExpression(
                                            Syntax.ThisExpression(),
                                            Syntax.IdentifierName(i.Name)),
                                        nameof(System.Object.GetHashCode)))))))

                });
        }

        IEnumerable<SyntaxNode> BuildAttributes(StructureType record)
        {
            yield break;

            //yield return BuildRecordAttribute(record);
            //yield return BuildStructLayoutAttribute(record);
        }

        SyntaxNode BuildRecordAttribute(StructureType record)
        {
            return Syntax.Attribute(
                typeof(RecordAttribute).FullName,
                Syntax.AttributeArgument(Syntax.LiteralExpression(record.Name)));
        }

        SyntaxNode BuildStructLayoutAttribute(StructureType record)
        {
            return Syntax.Attribute(
                typeof(StructLayoutAttribute).FullName,
                Syntax.AttributeArgument(
                    Syntax.MemberAccessExpression(
                        Syntax.DottedName(typeof(LayoutKind).FullName),
                        Syntax.IdentifierName(nameof(LayoutKind.Sequential)))));
        }

    }

}
