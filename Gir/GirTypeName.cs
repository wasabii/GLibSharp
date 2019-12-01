using System;

namespace Gir
{

    /// <summary>
    /// Describes a combination of namespace and name.
    /// </summary>
    public struct TypeName
    {

        /// <summary>
        /// Parses the given qualified name.
        /// </summary>
        /// <param name="qualifiedName"></param>
        /// <returns></returns>
        public static TypeName Parse(string qualifiedName, string defaultNamespace = null)
        {
            if (qualifiedName is null)
                throw new ArgumentNullException(nameof(qualifiedName));

            if (IsQualified(qualifiedName) == false)
                return new TypeName(defaultNamespace, qualifiedName);

            // strip off last segment of name
            var i = qualifiedName.LastIndexOf('.');
            return new TypeName(qualifiedName.Substring(0, i), qualifiedName.Substring(i + 1));
        }

        /// <summary>
        /// Returns <c>true</c> if the given name is qualified.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsQualified(string name) => name.Contains(".");

        public static implicit operator string(TypeName qn)
        {
            return qn.ToString();
        }

        readonly string ns;
        readonly string name;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="name"></param>
        public TypeName(string ns, string name)
        {
            this.ns = ns ?? throw new ArgumentNullException(nameof(ns));
            this.name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string Namespace => ns;

        public string Name => name;

        public override string ToString()
        {
            return ns + "." + name;
        }

    }

}
