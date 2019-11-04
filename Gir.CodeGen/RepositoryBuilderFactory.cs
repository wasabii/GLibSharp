using System;

using Microsoft.CodeAnalysis.Editing;

namespace Gir.CodeGen
{

    public class RepositoryBuilderFactory
    {

        readonly Func<SyntaxGenerator, ISyntaxBuilder> builderFunc;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="builderFunc"></param>
        public RepositoryBuilderFactory(Func<SyntaxGenerator, ISyntaxBuilder> builderFunc)
        {
            this.builderFunc = builderFunc;
        }

        public ISyntaxBuilder Create(SyntaxGenerator syntax)
        {
            return builderFunc(syntax);
        }

    }

}
