using System.Collections.Generic;

using Gir.Model;

using Microsoft.CodeAnalysis;

namespace Gir.CodeGen.Builders
{

    abstract class CallableBuilderBase<TElement> : SyntaxNodeBuilderBase<TElement>
        where TElement : Callable
    {

        protected abstract SyntaxNode BuildCallable(IContext context, TElement callable);

    }

}
