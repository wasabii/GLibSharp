using System.Collections.Generic;

using Microsoft.CodeAnalysis;

namespace Gir.CodeGen
{

    abstract class SyntaxNodeBuilderBase<TSymbol> : SyntaxNodeBuilderBase
        where TSymbol : ISymbol
    {

        protected virtual IEnumerable<SyntaxNode> Build(IContext context, TSymbol symbol)
        {
            yield break;
        }

        protected virtual SyntaxNode Adjust(IContext context, TSymbol symbol, SyntaxNode initial)
        {
            return initial;
        }

        public sealed override IEnumerable<SyntaxNode> Build(IContext context, ISymbol symbol)
        {
            return symbol is TSymbol s ? Build(context, s) : base.Build(context, symbol);
        }

        public sealed override SyntaxNode Adjust(IContext context, ISymbol symbol, SyntaxNode initial)
        {
            return symbol is TSymbol s ? Adjust(context, s, initial) : base.Adjust(context, symbol, initial);
        }

    }

    abstract class SyntaxNodeBuilderBase : ISyntaxNodeBuilder
    {

        public virtual IEnumerable<SyntaxNode> Build(IContext context, ISymbol symbol)
        {
            yield break;
        }

        public virtual SyntaxNode Adjust(IContext context, ISymbol symbol, SyntaxNode initial)
        {
            return initial;
        }

    }

}
