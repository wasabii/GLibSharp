using System;
using System.Collections.Generic;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes services available to an introspection element.
    /// </summary>
    public class IntrospectionContext
    {

        readonly TypeSymbolProvider symbols;
        readonly IList<(string Namespace, string Version)> imports;
        readonly string current;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="symbols"></param>
        /// <param name="imports"></param>
        /// <param name="current"></param>
        internal IntrospectionContext(TypeSymbolProvider symbols, IList<(string, string)> imports, string current)
        {
            this.symbols = symbols ?? throw new ArgumentNullException(nameof(symbols));
            this.imports = imports ?? throw new ArgumentNullException(nameof(imports));
            this.current = current;
        }

        /// <summary>
        /// Gets the current namespace of the context.
        /// </summary>
        public string CurrentNamespace => current;

        /// <summary>
        /// Attempts to resolve the given full or partial type name given the context.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public TypeSymbol ResolveSymbol(string name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            // some well known type names
            if (name == "" || name == "none")
                return null;

            // type name might be qualified initially
            if (QualifiedTypeName.IsQualified(name))
            {
                var qualifiedName = QualifiedTypeName.Parse(name);

                // check matching namespaces in reverse order (duplicates might exist by version)
                for (var i = imports.Count - 1; i >= 0; i--)
                    if (symbols.Resolve(imports[i].Namespace, imports[i].Version, qualifiedName.Name) is TypeSymbol s)
                        return s;

                // could not find, return null
                return null;
            }

            // check the imported namespaces in reverse order
            for (var i = imports.Count - 1; i >= 0; i--)
                if (symbols.Resolve(imports[i].Namespace, imports[i].Version, name) is TypeSymbol s)
                    return s;

            // could not locate, return null
            return null;
        }

    }

}
