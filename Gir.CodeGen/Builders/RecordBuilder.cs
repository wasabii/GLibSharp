using System.Collections.Generic;
using System.Xml.Linq;

using Gir.CodeGen.Symbols;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace Gir.CodeGen
{

    /// <summary>
    /// Builds the syntax for record elements.
    /// </summary>
    class RecordBuilder : SyntaxNodeBuilderBase<RecordSymbol>
    {

        protected override IEnumerable<SyntaxNode> Build(IContext context, RecordSymbol symbol)
        {
            yield return BuildRecord(context, symbol);
        }

        SyntaxNode BuildRecord(IContext context, RecordSymbol symbol) =>
            context.Syntax.ClassDeclaration(
                GetName(context, symbol),
                GetTypeParameters(context, symbol),
                GetAccessibility(context, symbol),
                GetModifiers(context, symbol),
                GetBaseType(context, symbol),
                GetInterfaceTypes(context, symbol),
                GetMembers(context, symbol))
            .NormalizeWhitespace();

        string GetName(IContext context, RecordSymbol symbol)
        {
            return symbol.Name.Name;
        }

        IEnumerable<string> GetTypeParameters(IContext context, RecordSymbol symbol)
        {
            yield break;
        }

        Accessibility GetAccessibility(IContext context, RecordSymbol symbol)
        {
            return Accessibility.Public;
        }

        DeclarationModifiers GetModifiers(IContext context, RecordSymbol symbol)
        {
            return DeclarationModifiers.Partial;
        }

        SyntaxNode? GetBaseType(IContext context, RecordSymbol symbol)
        {
            return default;
        }

        IEnumerable<SyntaxNode> GetInterfaceTypes(IContext context, RecordSymbol symbol)
        {
            yield break;
        }

        IEnumerable<SyntaxNode> GetMembers(IContext context, RecordSymbol symbol)
        {
            foreach (var i in element.Elements())
                foreach (var j in context.Build(i))
                    yield return j;
        }

        public SyntaxNode Adjust(IContext context, XElement element, RecordSymbol symbol)
        {
            return initial;
        }

    }

}
