using GObject.Introspection.Model;

using Microsoft.CodeAnalysis;

namespace GObject.Introspection.CodeGen.Builders
{

    abstract class CallableBuilderBase<TElement> : SyntaxNodeBuilderBase<TElement>
        where TElement : Callable
    {

        protected abstract SyntaxNode BuildCallable(IContext context, TElement callable);

    }

}
