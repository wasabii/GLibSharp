﻿using System;
using System.Collections.Generic;
using System.Linq;

using Gir.CodeGen.Builders;
using Gir.Model;

using Microsoft.CodeAnalysis;

namespace Gir.CodeGen
{

    /// <summary>
    /// Builds the syntax for class elements.
    /// </summary>
    class InterfaceBuilder : SyntaxNodeBuilderBase<Interface>
    {

        protected override IEnumerable<SyntaxNode> Build(IContext context, Interface iface)
        {
            yield return BuildInterface(context, iface);
        }

        SyntaxNode BuildInterface(IContext context, Interface iface) =>
            context.Syntax.AddAttributes(
                context.Syntax.InterfaceDeclaration(
                    GetName(context, iface),
                    BuildTypeParameters(context, iface),
                    GetAccessibility(context, iface),
                    BuildInterfaceTypes(context, iface),
                    BuildMembers(context, iface)),
                BuildAttributes(context, iface))
            .NormalizeWhitespace();

        string GetName(IContext context, Interface klass)
        {
            return context.ClrTypeInfo.Resolve(new GirTypeName(context.CurrentNamespace, klass.Name)).ClrTypeName;
        }

        IEnumerable<string> BuildTypeParameters(IContext context, Interface iface)
        {
            yield break;
        }

        Accessibility GetAccessibility(IContext context, Interface iface)
        {
            return Accessibility.Public;
        }

        IEnumerable<SyntaxNode> BuildInterfaceTypes(IContext context, Interface iface)
        {
            return iface.Implements.Select(i => BuildInterfaceType(context, iface, i));
        }

        SyntaxNode BuildInterfaceType(IContext context, Interface iface, Implements implements)
        {
            var typeName = GirTypeName.Parse(implements.Name, context.CurrentNamespace);
            var symbol = context.ClrTypeInfo.Resolve(typeName);
            if (symbol == null)
                throw new InvalidOperationException("Could not locate interface type.");

            return context.Syntax.DottedName(symbol.ClrTypeName);
        }

        IEnumerable<SyntaxNode> BuildMembers(IContext context, Interface iface)
        {
            // pass current type information to members
            var typeName = new GirTypeName(context.CurrentNamespace, iface.Name);
            var clrTypeName = context.ResolveClrTypeName(typeName);
            context = context.WithAnnotation(new CallableBuilderOptions(typeName, clrTypeName, true));

            foreach (var i in iface.Functions)
                foreach (var j in context.Build(i))
                    yield return j;

            foreach (var i in iface.Callbacks)
                foreach (var j in context.Build(i))
                    yield return j;

            foreach (var i in iface.Fields)
                foreach (var j in context.Build(i))
                    yield return j;

            foreach (var i in iface.Properties)
                foreach (var j in context.Build(i))
                    yield return j;

            foreach (var i in BuildMethods(context, iface))
                yield return i;

            foreach (var i in iface.Signals)
                foreach (var j in context.Build(i))
                    yield return j;
        }

        IEnumerable<SyntaxNode> BuildMethods(IContext context, Interface iface)
        {
            foreach (var i in iface.Methods.Where(i => !iface.VirtualMethods.Any(j => j.Invoker == i.Name)))
                foreach (var j in context.Build(i))
                    yield return j;

            foreach (var i in iface.VirtualMethods)
                foreach (var j in context.Build(i))
                    yield return j;
        }

        protected override IEnumerable<SyntaxNode> BuildAttributes(IContext context, Interface iface)
        {
            foreach (var attr in base.BuildAttributes(context, iface))
                yield return attr;

            yield return BuildFunctionAttribute(context, iface);
        }

        SyntaxNode BuildFunctionAttribute(IContext context, Interface iface)
        {
            return context.Syntax.Attribute(
                   typeof(InterfaceAttribute).FullName,
                   context.Syntax.AttributeArgument(context.Syntax.LiteralExpression(iface.Name)));
        }

    }

}
