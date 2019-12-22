using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

using GObject.Introspection.Xml;
using GObject.Introspection.Model;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace GObject.Introspection.CodeGen
{

    /// <summary>
    /// Builds the syntax for structure types.
    /// </summary>
    class StructureBuilder : SyntaxNodeBuilderBase<StructureType>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public StructureBuilder(SyntaxBuilderContext context) :
            base(context)
        {

        }

        protected override IEnumerable<SyntaxNode> Build(StructureType structure)
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
            return Accessibility.Public;
        }

        DeclarationModifiers GetModifiers(StructureType structure)
        {
            return DeclarationModifiers.Partial;
        }

        IEnumerable<SyntaxNode> BuildInterfaceTypes(StructureType structure)
        {
            return structure.ImplementedInterfaces.Select(i => Syntax.DottedName(i.Name));
        }

        IEnumerable<SyntaxNode> BuildMembers(StructureType structure)
        {
            foreach (var node in structure.Members.SelectMany(i => BuildMember(i)))
                yield return node;

            yield return BuildEqualsMethod(structure);
            yield return BuildTypedEqualsMethod(structure);
            yield return BuildGetHashCodeMethod(structure);
        }

        IEnumerable<SyntaxNode> BuildMember(Model.Member member)
        {
            return Context.Build(member);
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
                                        Syntax.IdentifierName(i.Name.ToPascalCase())),
                                    nameof(System.Object.Equals)),
                                Syntax.MemberAccessExpression(
                                    Syntax.IdentifierName("other"),
                                    Syntax.IdentifierName(i.Name.ToPascalCase()))))))
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
                                            Syntax.IdentifierName(i.Name.ToPascalCase())),
                                        nameof(System.Object.GetHashCode)))))))

                });
        }

        /// <summary>
        /// Builds the field.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="record"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        IEnumerable<SyntaxNode> BuildField(StructureType record, FieldElement field)
        {
            context = context.WithDebugText($"Field({field.Name})");

            if (field.Type != null)
                yield return BuildTypeField(context, record, field);
            if (field.Callback != null)
                context.Warning($"Callback field types are not yet implemented. Skipping {record}.{field}.");
        }

        /// <summary>
        /// Builds the field node.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="record"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        SyntaxNode BuildTypeField(StructureType record, FieldElement field) =>
            Context.AddAttributes(
                Context.FieldDeclaration(
                    GetFieldName(context, field),
                    BuildTypeFieldType(context, record, field),
                    GetFieldAccessibility(context, field),
                    GetFieldModifiers(context, field)),
                BuildFieldAttributes(context, field))
            .NormalizeWhitespace();

        /// <summary>
        /// Builds the field node.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="record"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        SyntaxNode BuildCallbackField(StructureType record, FieldElement field) =>
            Context.AddAttributes(
                Context.FieldDeclaration(
                    GetFieldName(field),
                    BuildCallbackFieldType(record, field),
                    GetFieldAccessibility(field),
                    GetFieldModifiers(field)),
                BuildFieldAttributes(field))
            .NormalizeWhitespace();

        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        string GetFieldName(StructureType field)
        {
            return field.Name;
        }

        /// <summary>
        /// Builds the syntax node representing the type of the field.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        SyntaxNode BuildTypeFieldType(IContext context, RecordElement record, FieldElement field)
        {
            if (record is null)
                throw new ArgumentNullException(nameof(record));
            if (field is null)
                throw new ArgumentNullException(nameof(field));

            // type should be specified for a field
            if (field.Type == null)
                throw new GirException($"Missing field type for {record}.{field}.");

            var type = GetFieldType(context, field, field.Type);
            if (type == null)
                throw new GirException("Unable to determine field type.");

            return type;
        }

        /// <summary>
        /// Builds the syntax node representing the type of a callback field.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        SyntaxNode BuildCallbackFieldType(StructureType record, FieldMember field)
        {
            if (record is null)
                throw new ArgumentNullException(nameof(record));
            if (field is null)
                throw new ArgumentNullException(nameof(field));

            // type should be specified for a field
            if (field.Callback == null)
                throw new GirException($"Missing callback type for {record}.{field}.");

            throw new NotImplementedException();
        }

        /// <summary>
        /// Resolves the specified type in relation to a field.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="field"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        SyntaxNode GetFieldType(FieldMember field)
        {
            return Context.DottedName(field.FieldType.Name);
        }

        Accessibility GetFieldAccessibility(FieldMember field)
        {
            return field.Writable == false ? Accessibility.Private : Accessibility.Public;
        }

        DeclarationModifiers GetFieldModifiers(FieldMember field)
        {
            return DeclarationModifiers.None;
        }

        IEnumerable<SyntaxNode> BuildFieldAttributes(FieldMember field)
        {
            yield break;
        }

        protected override IEnumerable<SyntaxNode> BuildAttributes(StructureType record)
        {
            yield return BuildRecordAttribute(context, record);
            yield return BuildStructLayoutAttribute(context, record);
        }

        SyntaxNode BuildRecordAttribute(StructureType record)
        {
            return context.Syntax.Attribute(
                typeof(RecordAttribute).FullName,
                context.Syntax.AttributeArgument(context.Syntax.LiteralExpression(record.Name)));
        }

        SyntaxNode BuildStructLayoutAttribute(StructureType record)
        {
            return context.Syntax.Attribute(
                typeof(StructLayoutAttribute).FullName,
                context.Syntax.AttributeArgument(
                    context.Syntax.MemberAccessExpression(
                        context.Syntax.DottedName(typeof(LayoutKind).FullName),
                        context.Syntax.IdentifierName(nameof(LayoutKind.Sequential)))));
        }

    }

}
