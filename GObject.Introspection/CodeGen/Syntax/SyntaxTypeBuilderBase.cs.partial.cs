using System;
using System.Collections.Generic;

using GObject.Introspection.Library.Model;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace GObject.Introspection.CodeGen.Syntax
{

    partial class SyntaxTypeBuilderBase
    {

        /// <summary>
        /// Applies the CSharp documentation to the member.
        /// </summary>
        /// <param name="member"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        SyntaxNode ApplyCSharpMemberDocumentation(SyntaxNode member, Documentation doc)
        {
            if (member is null)
                throw new ArgumentNullException(nameof(member));

            // apply documentation to the member
            if (doc?.Text != null)
            {
                var l = doc.Text.Split('\n');
                var s = new List<SyntaxToken>(l.Length * 2 + 4);

                s.Add(XmlTextNewLine("\n"));

                foreach (var i in l)
                {
                    s.Add(XmlTextLiteral(" "));
                    s.Add(XmlTextLiteral(i));
                    s.Add(XmlTextNewLine("\n"));
                }

                s.Add(XmlTextLiteral(" "));

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
