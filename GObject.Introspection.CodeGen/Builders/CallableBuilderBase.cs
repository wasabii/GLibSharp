using GObject.Introspection.Xml;

using Microsoft.CodeAnalysis;

namespace GObject.Introspection.CodeGen.Builders
{

    abstract class CallableBuilderBase<TElement> : SyntaxNodeBuilderBase<TElement>
        where TElement : CallableElement
    {

        protected abstract SyntaxNode BuildCallable(IContext context, TElement callable);

    }

}
