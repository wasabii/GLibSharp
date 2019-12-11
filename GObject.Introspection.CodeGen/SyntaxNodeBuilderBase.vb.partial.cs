using System;
using System.Linq;

using GObject.Introspection.Model;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

using static Microsoft.CodeAnalysis.VisualBasic.SyntaxFactory;

namespace GObject.Introspection.CodeGen
{

    partial class SyntaxNodeBuilderBase
    {

        /// <summary>
        /// Applies the Visual Basic documentation to the member.
        /// </summary>
        /// <param name="member"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        SyntaxNode ApplyVisualBasicMemberDocumentation(SyntaxNode member, Documentation doc)
        {
            if (member is null)
                throw new ArgumentNullException(nameof(member));

            // apply documentation to the member
            if (doc?.Text != null)
            {
                var s = doc.Text.Split('\n').Select(line => XmlTextLiteral(line)).ToList();
                for (var i = 1; i < s.Count; i += 2)
                    s.Insert(i, XmlTextNewLine("\n"));

                member = member.WithLeadingTrivia(
                    TriviaList(
                        Trivia(
                            DocumentationComment(
                                XmlElement("summary", SingletonList((XmlNodeSyntax)XmlText(TokenList(s)))),
                                XmlText("\n")))));
            }

            return member;
        }

    }

}
