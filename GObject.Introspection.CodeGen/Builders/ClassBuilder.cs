using System;
using System.Collections.Generic;
using System.Linq;

using GObject.Introspection.CodeGen.Builders;
using GObject.Introspection.Model;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace GObject.Introspection.CodeGen
{

    /// <summary>
    /// Builds the syntax for class elements.
    /// </summary>
    class ClassBuilder : SyntaxNodeBuilderBase<Class>
    {

        protected override IEnumerable<SyntaxNode> Build(IContext context, Class klass)
        {
            // records mapped to CLR types explicitly do not get built
            var clrInfo = context.ResolveTypeInfo(new QualifiedTypeName(context.CurrentNamespace, klass.Name));
            if (clrInfo?.ClrTypeExpression != null)
                yield break;

            yield return BuildClass(context, klass);
        }

        SyntaxNode BuildClass(IContext context, Class klass) =>
            context.Syntax.AddAttributes(
                context.Syntax.ClassDeclaration(
                    GetName(context, klass),
                    BuildTypeParameters(context, klass),
                    GetAccessibility(context, klass),
                    GetModifiers(context, klass),
                    BuildBaseType(context, klass),
                    BuildInterfaceTypes(context, klass),
                    BuildMembers(context, klass)),
                BuildAttributes(context, klass))
            .NormalizeWhitespace();

        string GetName(IContext context, Class klass)
        {
            return klass.Name;
        }

        IEnumerable<string> BuildTypeParameters(IContext context, Class klass)
        {
            yield break;
        }

        Accessibility GetAccessibility(IContext context, Class klass)
        {
            return Accessibility.Public;
        }

        DeclarationModifiers GetModifiers(IContext context, Class klass)
        {
            var modifiers = DeclarationModifiers.Partial;

            if (klass.Abstract == true)
                modifiers |= DeclarationModifiers.Abstract;

            return modifiers;
        }

        SyntaxNode BuildBaseType(IContext context, Class klass)
        {
            if (klass.Parent == null)
                return null;

            var typeName = QualifiedTypeName.Parse(klass.Parent, context.CurrentNamespace);
            var typeInfo = context.ResolveTypeInfo(typeName);
            if (typeInfo == null)
                throw new GirException($"Coult not resolve base type {typeName}.");

            // use type specification to get CLR type
            var typeSpec = new TypeSpec(typeInfo);
            return typeSpec.GetClrTypeExpression(context.Syntax);
        }

        IEnumerable<SyntaxNode> BuildInterfaceTypes(IContext context, Class klass)
        {
            return klass.Implements.Select(i => BuildInterfaceType(context, klass, i));
        }

        SyntaxNode BuildInterfaceType(IContext context, Class klass, Implements implements)
        {
            var typeName = QualifiedTypeName.Parse(implements.Name, context.CurrentNamespace);

            var typeInfo = context.ResolveTypeInfo(typeName);
            if (typeInfo == null)
                throw new GirException("Could not resolve type info for class.");

            return context.Syntax.DottedName(typeInfo.Name);
        }

        IEnumerable<SyntaxNode> BuildMembers(IContext context, Class klass)
        {
            // pass current type information to members
            var typeName = new QualifiedTypeName(context.CurrentNamespace, klass.Name);
            var typeInfo = context.ResolveTypeInfo(typeName);
            context = context.WithAnnotation(new CallableBuilderOptions(typeInfo, true));

            foreach (var i in klass.Unions)
                foreach (var j in context.Build(i))
                    yield return j;

            foreach (var i in klass.Records)
                foreach (var j in context.Build(i))
                    yield return j;

            foreach (var i in klass.Functions)
                foreach (var j in context.Build(i))
                    yield return j;

            foreach (var i in klass.Callbacks)
                foreach (var j in context.Build(i))
                    yield return j;

            foreach (var i in klass.Fields)
                foreach (var j in context.Build(i))
                    yield return j;

            foreach (var i in BuildHandleMembers(context, klass))
                yield return i;

            foreach (var i in klass.Constructors)
                foreach (var j in context.Build(i))
                    yield return j;

            foreach (var i in klass.Properties)
                foreach (var j in context.Build(i))
                    yield return j;

            foreach (var i in BuildMethods(context, klass))
                yield return i;

            foreach (var i in klass.Signals)
                foreach (var j in context.Build(i))
                    yield return j;
        }

        IEnumerable<SyntaxNode> BuildHandleMembers(IContext context, Class klass)
        {
            yield return BuildHandleConstructor(context, klass);
        }

        SyntaxNode BuildHandleConstructor(IContext context, Class klass)
        {
            return context.Syntax.ConstructorDeclaration(
                GetName(context, klass),
                BuildHandleConstructorParameters(context, klass),
                Accessibility.Internal,
                DeclarationModifiers.None,
                BuildHandleConstructorBaseArguments(context, klass),
                BuildHandleConstructorStatements(context, klass));
        }

        IEnumerable<SyntaxNode> BuildHandleConstructorParameters(IContext context, Class klass)
        {
            yield return context.Syntax.ParameterDeclaration("handle", context.Syntax.DottedName(typeof(IntPtr).FullName));
        }

        IEnumerable<SyntaxNode> BuildHandleConstructorBaseArguments(IContext context, Class klass)
        {
            yield return context.Syntax.Argument(context.Syntax.IdentifierName("handle"));
        }

        IEnumerable<SyntaxNode> BuildHandleConstructorStatements(IContext context, Class klass)
        {
            yield break;
        }

        IEnumerable<SyntaxNode> BuildMethods(IContext context, Class klass)
        {
            // build methods that are not referenced by a virtual method
            foreach (var i in klass.Methods.Where(i => !klass.VirtualMethods.Any(j => j.Invoker == i.Name)))
                foreach (var j in context.Build(i))
                    yield return j;

            foreach (var i in klass.VirtualMethods)
                foreach (var j in context.Build(i))
                    yield return j;
        }

        protected override IEnumerable<SyntaxNode> BuildAttributes(IContext context, Class klass)
        {
            foreach (var attr in base.BuildAttributes(context, klass))
                yield return attr;

            yield return BuildClassAttribute(context, klass);
        }

        SyntaxNode BuildClassAttribute(IContext context, Class klass)
        {
            return context.Syntax.Attribute(
                typeof(ClassAttribute).FullName,
                context.Syntax.AttributeArgument(context.Syntax.LiteralExpression(klass.Name)));
        }

    }

}
