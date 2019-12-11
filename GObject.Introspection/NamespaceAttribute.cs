using System;

namespace GObject.Introspection
{

    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class NamespaceAttribute : Attribute
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        public NamespaceAttribute(string name)
        {
            Name = name;
        }
        public string Name { get; }

        public string Version { get; set; }

        public string CPrefix { get; set; }

        public string CSymbolPrefixes { get; set; }

        public string CIdentifierPrefixes { get; set; }

        public string ClrSharedLibrary { get; set; }

    }

}
