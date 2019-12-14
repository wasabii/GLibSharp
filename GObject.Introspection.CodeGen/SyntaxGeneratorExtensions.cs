using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace GObject.Introspection.CodeGen
{

    /// <summary>
    /// Provides various extensions to <see cref="SyntaxGenerator"/>.
    /// </summary>
    public static class SyntaxGeneratorExtensions
    {

        static SyntaxNode CSharpExclusiveOrExpression(SyntaxGenerator self, SyntaxNode a, SyntaxNode b)
        {
            if (a == null || b == null)
                return a ?? b;

            return Microsoft.CodeAnalysis.CSharp.SyntaxFactory.BinaryExpression(
                Microsoft.CodeAnalysis.CSharp.SyntaxKind.ExclusiveOrExpression,
                (Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax)a,
                (Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax)b);
        }

        static SyntaxNode VisualBasicExclusiveOrExpression(SyntaxGenerator self, SyntaxNode a, SyntaxNode b)
        {
            if (a == null || b == null)
                return a ?? b;

            return Microsoft.CodeAnalysis.VisualBasic.SyntaxFactory.ExclusiveOrExpression(
                (Microsoft.CodeAnalysis.VisualBasic.Syntax.ExpressionSyntax)a,
                (Microsoft.CodeAnalysis.VisualBasic.Syntax.ExpressionSyntax)b);
        }

        /// <summary>
        /// Generates an exclusive binary or operation expression.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static SyntaxNode ExclusiveOrExpression(this SyntaxGenerator self, SyntaxNode a, SyntaxNode b)
        {
            if (self is null)
                throw new ArgumentNullException(nameof(self));

            return (self.IdentifierName("_")) switch
            {
                Microsoft.CodeAnalysis.CSharp.Syntax.IdentifierNameSyntax _ => CSharpExclusiveOrExpression(self, a, b),
                Microsoft.CodeAnalysis.VisualBasic.Syntax.IdentifierNameSyntax _ => VisualBasicExclusiveOrExpression(self, a, b),
                _ => throw new NotSupportedException(),
            };
        }

        static SyntaxNode Join(SyntaxGenerator self, IEnumerable<SyntaxNode> nodes, Func<SyntaxNode, SyntaxNode, SyntaxNode> join)
        {
            if (self is null)
                throw new ArgumentNullException(nameof(self));

            return nodes.Aggregate((SyntaxNode)null, (a, b) => a != null && b != null ? join(a, b) : a ?? b);
        }

        /// <summary>
        /// Creates an expression that denotes a logical-and expression.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="nodes"></param>
        /// <returns></returns>
        public static SyntaxNode LogicalAndExpression(this SyntaxGenerator self, IEnumerable<SyntaxNode> nodes)
        {
            if (self is null)
                throw new ArgumentNullException(nameof(self));
            if (nodes is null)
                throw new ArgumentNullException(nameof(nodes));

            return Join(self, nodes, self.LogicalAndExpression);
        }

        /// <summary>
        /// Creates an expression that denotes a exclusive-or expression.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="nodes"></param>
        /// <returns></returns>
        public static SyntaxNode LogicalOrExpression(this SyntaxGenerator self, IEnumerable<SyntaxNode> nodes)
        {
            if (self is null)
                throw new ArgumentNullException(nameof(self));
            if (nodes is null)
                throw new ArgumentNullException(nameof(nodes));

            return Join(self, nodes, self.LogicalOrExpression);
        }

        /// <summary>
        /// Creates an expression that denotes a exclusive-or expression.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="nodes"></param>
        /// <returns></returns>
        public static SyntaxNode ExclusiveOrExpression(this SyntaxGenerator self, IEnumerable<SyntaxNode> nodes)
        {
            if (self is null)
                throw new ArgumentNullException(nameof(self));
            if (nodes is null)
                throw new ArgumentNullException(nameof(nodes));

            return Join(self, nodes, self.ExclusiveOrExpression);
        }

    }

}
