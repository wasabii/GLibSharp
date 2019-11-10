using System;
using System.Collections.Generic;
using System.Linq;

using Gir.Model;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace Gir.CodeGen
{

    /// <summary>
    /// Builds the syntax for class elements.
    /// </summary>
    class NamespaceBuilder : SyntaxNodeBuilderBase<Namespace>
    {

        protected override IEnumerable<SyntaxNode> Build(IContext context, Namespace symbol)
        {
            foreach (var attr in BuildAssemblyAttributes(context, symbol))
                yield return attr;

            yield return BuildNamespace(context, symbol);
        }

        IEnumerable<SyntaxNode> BuildAssemblyAttributes(IContext context, Namespace symbol)
        {
            return BuildPrimitives(context, symbol);
        }

        SyntaxNode BuildNamespace(IContext context, Namespace symbol) =>
            context.Syntax.NamespaceDeclaration(
                symbol.Name,
                BuildMembers(context, symbol).OfType<SyntaxNode>());

        IEnumerable<SyntaxNode> BuildMembers(IContext context, Namespace symbol)
        {
            return Enumerable.Empty<SyntaxNode>()
                .Concat(BuildAliases(context, symbol))
                .Concat(BuildBitFields(context, symbol))
                .Concat(BuildCallbacks(context, symbol))
                .Concat(BuildClasses(context, symbol))
                .Concat(BuildConstants(context, symbol))
                .Concat(BuildEnums(context, symbol))
                .Concat(BuildFunctions(context, symbol))
                .Concat(BuildInterfaces(context, symbol))
                .Concat(BuildRecords(context, symbol))
                .Concat(BuildUnions(context, symbol));
        }

        IEnumerable<SyntaxNode> BuildPrimitives(IContext context, Namespace symbol)
        {
            return BuildElements(context, symbol.Primitives, symbol);
        }

        IEnumerable<SyntaxNode> BuildAliases(IContext context, Namespace symbol)
        {
            return BuildElements(context, symbol.Aliases, symbol);
        }

        IEnumerable<SyntaxNode> BuildBitFields(IContext context, Namespace symbol)
        {
            return BuildElements(context, symbol.BitFields, symbol);
        }

        IEnumerable<SyntaxNode> BuildCallbacks(IContext context, Namespace symbol)
        {
            return BuildElements(context, symbol.Callbacks, symbol);
        }

        IEnumerable<SyntaxNode> BuildClasses(IContext context, Namespace symbol)
        {
            return BuildElements(context, symbol.Classes, symbol);
        }

        IEnumerable<SyntaxNode> BuildConstants(IContext context, Namespace symbol)
        {
            return BuildElements(context, symbol.Constants, symbol);
        }

        IEnumerable<SyntaxNode> BuildEnums(IContext context, Namespace symbol)
        {
            return BuildElements(context, symbol.Enums, symbol);
        }

        IEnumerable<SyntaxNode> BuildFunctions(IContext context, Namespace symbol)
        {
            yield return context.Syntax.ClassDeclaration(
                "Functions",
                accessibility: Accessibility.Public,
                modifiers: DeclarationModifiers.Partial | DeclarationModifiers.Static,
                members: BuildElements(context, symbol.Functions, symbol));
        }

        IEnumerable<SyntaxNode> BuildInterfaces(IContext context, Namespace symbol)
        {
            return BuildElements(context, symbol.Interfaces, symbol);
        }

        IEnumerable<SyntaxNode> BuildRecords(IContext context, Namespace symbol)
        {
            return BuildElements(context, symbol.Records, symbol);
        }

        IEnumerable<SyntaxNode> BuildUnions(IContext context, Namespace symbol)
        {
            return BuildElements(context, symbol.Unions, symbol);
        }

        IEnumerable<SyntaxNode> BuildElements<TElement>(IContext context, IEnumerable<TElement> elements, Namespace ns)
            where TElement : Element
        {
            return elements.SelectMany(i => context.WithNamespace(defaultNamespace: ns.Name).Build(i));
        }

    }

}
