using System;
using System.Linq;

using Gir.Model;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Gir.CodeGen
{

    partial class SyntaxNodeBuilderBase
    {

        /// <summary>
        /// Applies the CSharp documentation to the member.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="member"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        SyntaxNode ApplyCSharpMemberDocumentation(IContext context, SyntaxNode member, Documentation doc)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));
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
