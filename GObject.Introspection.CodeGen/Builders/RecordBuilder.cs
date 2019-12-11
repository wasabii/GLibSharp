using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

using GObject.Introspection.CodeGen.Builders;
using GObject.Introspection.Model;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace GObject.Introspection.CodeGen
{

    /// <summary>
    /// Builds the syntax for record elements.
    /// </summary>
    class RecordBuilder : SyntaxNodeBuilderBase<Record>
    {

        protected override IEnumerable<SyntaxNode> Build(IContext context, Record record)
        {
            // internal type structs not exposed
            if (string.IsNullOrEmpty(record.GLibIsGTypeStructFor) == false)
                yield break;

            yield return BuildRecord(context, record);
        }

        SyntaxNode BuildRecord(IContext context, Record record) =>
            context.Syntax.AddAttributes(
                context.Syntax.StructDeclaration(
                    GetName(context, record),
                    BUildTypeParameters(context, record),
                    GetAccessibility(context, record),
                    GetModifiers(context, record),
                    BuildInterfaceTypes(context, record),
                    BuildMembers(context, record)),
                BuildAttributes(context, record))
            .NormalizeWhitespace();

        string GetName(IContext context, Record record)
        {
            return record.Name;
        }

        IEnumerable<string> BUildTypeParameters(IContext context, Record record)
        {
            yield break;
        }

        Accessibility GetAccessibility(IContext context, Record record)
        {
            return Accessibility.Public;
        }

        DeclarationModifiers GetModifiers(IContext context, Record record)
        {
            return DeclarationModifiers.Partial;
        }

        IEnumerable<SyntaxNode> BuildInterfaceTypes(IContext context, Record record)
        {
            yield return context.Syntax.GenericName(
                "System.IEquatable",
                context.Syntax.IdentifierName(record.Name));
        }

        IEnumerable<SyntaxNode> BuildMembers(IContext context, Record record)
        {
            // pass current type information to members
            var typeName = new QualifiedTypeName(context.CurrentNamespace, record.Name);
            var typeInfo = context.ResolveTypeInfo(typeName);
            context = context.WithAnnotation(new CallableBuilderOptions(typeInfo, false));

            foreach (var i in record.Unions)
                foreach (var j in context.Build(i))
                    yield return j;

            foreach (var i in record.Constructors)
                foreach (var j in context.Build(i))
                    yield return j;

            foreach (var i in record.Functions)
                foreach (var j in context.Build(i))
                    yield return j;

            foreach (var i in record.Fields)
                foreach (var j in BuildField(context, record, i))
                    yield return j;

            foreach (var i in record.Methods)
                foreach (var j in context.Build(i))
                    yield return j;

            yield return BuildEqualsMethod(context, record);
            yield return BuildTypedEqualsMethod(context, record);
            yield return BuildGetHashCodeMethod(context, record);
        }

        SyntaxNode BuildEqualsMethod(IContext context, Record record)
        {
            return context.Syntax.MethodDeclaration(
                nameof(System.Object.Equals),
                new[]
                {
                    context.Syntax.ParameterDeclaration(
                        "other",
                        context.Syntax.TypeExpression(SpecialType.System_Object))
                },
                null,
                context.Syntax.TypeExpression(SpecialType.System_Boolean),
                Accessibility.Public,
                DeclarationModifiers.Override,
                new[]
                {
                    context.Syntax.ReturnStatement(
                        context.Syntax.ConditionalExpression(
                            context.Syntax.LogicalNotExpression(
                                context.Syntax.IsTypeExpression(
                                    context.Syntax.IdentifierName("other"),
                                    context.Syntax.IdentifierName(record.Name))),
                            context.Syntax.FalseLiteralExpression(),
                            context.Syntax.InvocationExpression(
                                context.Syntax.MemberAccessExpression(
                                    context.Syntax.ThisExpression(),
                                    context.Syntax.IdentifierName(nameof(System.Object.Equals))),
                                context.Syntax.CastExpression(
                                    context.Syntax.IdentifierName(record.Name),
                                    context.Syntax.IdentifierName("other")))))
                });
        }

        SyntaxNode BuildTypedEqualsMethod(IContext context, Record record)
        {
            return context.Syntax.MethodDeclaration(
                nameof(System.Object.Equals),
                new[]
                {
                    context.Syntax.ParameterDeclaration(
                        "other",
                        context.Syntax.IdentifierName(record.Name))
                },
                null,
                context.Syntax.TypeExpression(SpecialType.System_Boolean),
                Accessibility.Public,
                DeclarationModifiers.None,
                new[]
                {
                    context.Syntax.ReturnStatement(
                        context.Syntax.LogicalAndExpression(record.Fields.Select(i =>
                            context.Syntax.InvocationExpression(
                                context.Syntax.MemberAccessExpression(
                                    context.Syntax.MemberAccessExpression(
                                        context.Syntax.ThisExpression(),
                                        context.Syntax.IdentifierName(i.Name.ToPascalCase())),
                                    nameof(System.Object.Equals)),
                                context.Syntax.MemberAccessExpression(
                                    context.Syntax.IdentifierName("other"),
                                    context.Syntax.IdentifierName(i.Name.ToPascalCase()))))))
                });
        }

        SyntaxNode BuildGetHashCodeMethod(IContext context, Record record)
        {
            return context.Syntax.MethodDeclaration(
                nameof(System.Object.GetHashCode),
                null,
                null,
                context.Syntax.TypeExpression(SpecialType.System_Int32),
                Accessibility.Public,
                DeclarationModifiers.Override,
                new[]
                {
                    context.Syntax.ReturnStatement(
                        context.Syntax.ExclusiveOrExpression(
                            context.Syntax.InvocationExpression(
                                context.Syntax.MemberAccessExpression(
                                    context.Syntax.MemberAccessExpression(
                                        context.Syntax.InvocationExpression(
                                            context.Syntax.MemberAccessExpression(
                                                context.Syntax.ThisExpression(),
                                                nameof(System.Type.GetType))),
                                        nameof(System.Type.FullName)),
                                    nameof(System.Object.GetHashCode))),
                            context.Syntax.ExclusiveOrExpression(record.Fields.Select(i =>
                                context.Syntax.InvocationExpression(
                                    context.Syntax.MemberAccessExpression(
                                        context.Syntax.MemberAccessExpression(
                                            context.Syntax.ThisExpression(),
                                            context.Syntax.IdentifierName(i.Name.ToPascalCase())),
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
        IEnumerable<SyntaxNode> BuildField(IContext context, Record record, Field field)
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
        SyntaxNode BuildTypeField(IContext context, Record record, Field field) =>
            context.Syntax.AddAttributes(
                context.Syntax.FieldDeclaration(
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
        SyntaxNode BuildCallbackField(IContext context, Record record, Field field) =>
            context.Syntax.AddAttributes(
                context.Syntax.FieldDeclaration(
                    GetFieldName(context, field),
                    BuildCallbackFieldType(context, record, field),
                    GetFieldAccessibility(context, field),
                    GetFieldModifiers(context, field)),
                BuildFieldAttributes(context, field))
            .NormalizeWhitespace();

        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        string GetFieldName(IContext context, Field field)
        {
            return field.Name.ToPascalCase();
        }

        /// <summary>
        /// Builds the syntax node representing the type of the field.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        SyntaxNode BuildTypeFieldType(IContext context, Record record, Field field)
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
        SyntaxNode BuildCallbackFieldType(IContext context, Record record, Field field)
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
        SyntaxNode GetFieldType(IContext context, Field field, AnyType type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            var typeSpec = BuilderUtil.GetTypeSpec(context, type);
            if (typeSpec == null)
                throw new GirException("Unable to determine type specification.");

            return typeSpec.GetClrTypeExpression(context.Syntax);
        }

        Accessibility GetFieldAccessibility(IContext context, Field field)
        {
            return field.Writable == false ? Accessibility.Private : Accessibility.Public;
        }

        DeclarationModifiers GetFieldModifiers(IContext context, Field field)
        {
            return DeclarationModifiers.None;
        }

        IEnumerable<SyntaxNode> BuildFieldAttributes(IContext context, Field field)
        {
            yield break;
        }

        protected override IEnumerable<SyntaxNode> BuildAttributes(IContext context, Record record)
        {
            foreach (var attr in base.BuildAttributes(context, record))
                yield return attr;

            yield return BuildRecordAttribute(context, record);
            yield return BuildStructLayoutAttribute(context, record);
        }

        SyntaxNode BuildRecordAttribute(IContext context, Record record)
        {
            return context.Syntax.Attribute(
                typeof(RecordAttribute).FullName,
                context.Syntax.AttributeArgument(context.Syntax.LiteralExpression(record.Name)));
        }

        SyntaxNode BuildStructLayoutAttribute(IContext context, Record record)
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
