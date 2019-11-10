using System;

namespace Gir.CodeGen
{

    /// <summary>
    /// Describes a combination of namespace and name.
    /// </summary>
    public struct ClrTypeName
    {

        /// <summary>
        /// Parses the given qualified name.
        /// </summary>
        /// <param name="qualifiedName"></param>
        /// <returns></returns>
        public static ClrTypeName Parse(string qualifiedName, string defaultNamespace = null)
        {
            if (qualifiedName is null)
                throw new ArgumentNullException(nameof(qualifiedName));

            if (IsQualified(qualifiedName) == false)
            {
                // required if not qualified
                if (defaultNamespace is null)
                    throw new ArgumentNullException(nameof(defaultNamespace));

                return new ClrTypeName(defaultNamespace, qualifiedName);
            }

            // strip off last segment of name
            var i = qualifiedName.LastIndexOf('.');
            return new ClrTypeName(qualifiedName.Substring(0, i), qualifiedName.Substring(i + 1));
        }

        /// <summary>
        /// Returns <c>true</c> if the given name is qualified.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsQualified(string name) => name.Contains(".");

        public static implicit operator string(ClrTypeName qn)
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
        public ClrTypeName(string ns, string name)
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
