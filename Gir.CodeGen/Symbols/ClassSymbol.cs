using System;

using Gir.Xml;

namespace Gir.CodeGen.Symbols
{

    /// <summary>
    /// 
    /// </summary>
    class ClassSymbol : ISymbol
    {

        readonly ISymbolResolver resolver;
        readonly string ns;
        readonly Class klass;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        /// <param name="ns"></param>
        /// <param name="klass"></param>
        public ClassSymbol(ISymbolResolver resolver, string ns, Class klass)
        {
            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            this.ns = ns ?? throw new ArgumentNullException(nameof(ns));
            this.klass = klass ?? throw new ArgumentNullException(nameof(klass));
        }

        public SymbolName Name => new SymbolName(ns, klass.Name);

        public bool Deprecated => klass.Info.Deprecated == true;

    }

}
