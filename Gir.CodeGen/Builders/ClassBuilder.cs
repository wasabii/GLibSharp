using System;
using System.Collections.Generic;
using System.Linq;

using Gir.CodeGen.Builders;
using Gir.Model;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace Gir.CodeGen
{

    /// <summary>
    /// Builds the syntax for class elements.
    /// </summary>
    class ClassBuilder : SyntaxNodeBuilderBase<Class>
    {

        protected override IEnumerable<SyntaxNode> Build(IContext context, Class klass)
        {
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
            return context.ClrTypeInfo.Resolve(new GirTypeName(context.CurrentNamespace, klass.Name)).ClrTypeName;
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

            var parentTypeName = GirTypeName.Parse(klass.Parent, context.CurrentNamespace);
            var parentSymbol = context.ClrTypeInfo.Resolve(parentTypeName);
            if (parentSymbol == null)
                throw new InvalidOperationException("Could not locate base type.");

            return context.Syntax.DottedName(parentSymbol.ClrTypeName);
        }

        IEnumerable<SyntaxNode> BuildInterfaceTypes(IContext context, Class klass)
        {
            return klass.Implements.Select(i => BuildInterfaceType(context, klass, i));
        }

        SyntaxNode BuildInterfaceType(IContext context, Class klass, Implements implements)
        {
            var typeName = GirTypeName.Parse(implements.Name, context.CurrentNamespace);
            var symbol = context.ClrTypeInfo.Resolve(typeName);
            if (symbol == null)
                throw new InvalidOperationException("Could not locate interface type.");

            return context.Syntax.DottedName(symbol.ClrTypeName);
        }

        IEnumerable<SyntaxNode> BuildMembers(IContext context, Class klass)
        {
            // pass current type information to members
            var typeName = new GirTypeName(context.CurrentNamespace, klass.Name);
            var clrTypeName = context.ResolveClrTypeName(typeName);
            context = context.WithAnnotation(new CallableBuilderOptions(typeName, clrTypeName, true));

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
