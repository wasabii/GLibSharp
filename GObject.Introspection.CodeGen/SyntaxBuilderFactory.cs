using System;

using Microsoft.CodeAnalysis.Editing;

namespace GObject.Introspection.CodeGen
{

    public class SyntaxBuilderFactory
    {

        readonly Func<SyntaxGenerator, ISyntaxBuilder> builderFunc;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="builderFunc"></param>
        public SyntaxBuilderFactory(Func<SyntaxGenerator, ISyntaxBuilder> builderFunc)
        {
            this.builderFunc = builderFunc;
        }

        public ISyntaxBuilder Create(SyntaxGenerator syntax)
        {
            return builderFunc(syntax);
        }

    }

}
