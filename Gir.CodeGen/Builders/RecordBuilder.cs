using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Gir.CodeGen.Builders;
using Gir.Model;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace Gir.CodeGen
{

    /// <summary>
    /// Builds the syntax for record elements.
    /// </summary>
    class RecordBuilder : SyntaxNodeBuilderBase<Record>
    {

        protected override IEnumerable<SyntaxNode> Build(IContext context, Record record)
        {
            // records mapped to CLR types explicitly do not get built
            var clrInfo = context.ResolveTypeInfo(new TypeName(context.CurrentNamespace, record.Name));
            if (clrInfo?.ClrTypeExpression != null)
                yield break;

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
            yield break;
        }

        IEnumerable<SyntaxNode> BuildMembers(IContext context, Record record)
        {
            // pass current type information to members
            var typeName = new TypeName(context.CurrentNamespace, record.Name);
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
                foreach (var j in context.Build(i))
                    yield return j;

            foreach (var i in record.Methods)
                foreach (var j in context.Build(i))
                    yield return j;
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
