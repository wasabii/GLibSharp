using System;

namespace GObject.Introspection.CodeGen
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
        public static ClrTypeName Parse(string value)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            var namespaceNameIndex = value.LastIndexOf('.');
            var namespaceName = namespaceNameIndex > -1 ? value.Substring(0, namespaceNameIndex) : null;
            var name = namespaceNameIndex > -1 ? value.Substring(namespaceNameIndex + 1) : value;

            if (namespaceName is null)
                throw new Exception("Unable to parse CLR type name. Missing namespace.");

            return new ClrTypeName(namespaceName, name);
        }

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
