using System;

namespace Gir.CodeGen
{

    public struct SymbolName
    {

        /// <summary>
        /// Parses the given qualified name.
        /// </summary>
        /// <param name="qualifiedName"></param>
        /// <returns></returns>
        public static SymbolName Parse(string qualifiedName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns <c>true</c> if the given name is qualified.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsQualified(string name) => name.Contains(".");

        readonly string ns;
        readonly string name;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="name"></param>
        public SymbolName(string ns, string name)
        {
            this.ns = ns ?? throw new ArgumentNullException(nameof(ns));
            this.name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string Namespace => ns;

        public string Name => name;

    }

}
