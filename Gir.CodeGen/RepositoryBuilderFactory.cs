using System;

using Microsoft.CodeAnalysis.Editing;

namespace Gir.CodeGen
{

    public class RepositoryBuilderFactory
    {

        readonly Func<SyntaxGenerator, IRepositoryBuilder> builderFunc;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="builderFunc"></param>
        public RepositoryBuilderFactory(Func<SyntaxGenerator, IRepositoryBuilder> builderFunc)
        {
            this.builderFunc = builderFunc;
        }

        public IRepositoryBuilder Create(SyntaxGenerator syntax)
        {
            return builderFunc(syntax);
        }

    }

}
