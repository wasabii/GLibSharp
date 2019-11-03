using System.Collections.Generic;
using System.Xml.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace Gir.CodeGen
{

    /// <summary>
    /// Builds the syntax for record elements.
    /// </summary>
    class RecordProcessor : IProcessor
    {

        public IEnumerable<SyntaxNode> Build(IContext context, XElement element)
        {
            if (element.Name == Xmlns.Core_1_0 + "record")
                yield return BuildRecord(context, element);
        }

        SyntaxNode BuildRecord(IContext context, XElement element) =>
            context.Syntax.ClassDeclaration(
                GetName(context, element),
                GetTypeParameters(context, element),
                GetAccessibility(context, element),
                GetModifiers(context, element),
                GetBaseType(context, element),
                GetInterfaceTypes(context, element),
                GetMembers(context, element))
            .NormalizeWhitespace();

        string GetName(IContext context, XElement element)
        {
            return (string)element.Attribute("name");
        }

        IEnumerable<string> GetTypeParameters(IContext context, XElement element)
        {
            yield break;
        }

        Accessibility GetAccessibility(IContext context, XElement element)
        {
            return Accessibility.Public;
        }

        DeclarationModifiers GetModifiers(IContext context, XElement element)
        {
            return DeclarationModifiers.Partial;
        }

        SyntaxNode? GetBaseType(IContext context, XElement element)
        {
            return default;
        }

        IEnumerable<SyntaxNode> GetInterfaceTypes(IContext context, XElement element)
        {
            yield break;
        }

        IEnumerable<SyntaxNode> GetMembers(IContext context, XElement element)
        {
            foreach (var i in element.Elements())
                foreach (var j in context.Build(i))
                    yield return j;
        }

        public SyntaxNode Adjust(IContext context, XElement element, SyntaxNode initial)
        {
            return initial;
        }

    }

}
