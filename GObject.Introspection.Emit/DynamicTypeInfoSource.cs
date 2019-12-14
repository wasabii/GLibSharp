using System;
using System.Reflection;
using System.Reflection.Emit;

using GObject.Introspection.Reflection;

namespace GObject.Introspection.Dynamic
{

    /// <summary>
    /// Provides a type info source across an existing loaded introspection library.
    /// </summary>
    class DynamicTypeInfoSource : ITypeInfoSource
    {

        readonly ModuleBuilder module;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module"></param>
        public DynamicTypeInfoSource(ModuleBuilder module)
        {
            this.module = module ?? throw new ArgumentNullException(nameof(module));
        }

        /// <summary>
        /// Resolves the specified type name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public TypeInfo ResolveTypeInfo(TypeSymbol symbol)
        {
            if (symbol is null)
                throw new ArgumentNullException(nameof(symbol));

            // recurse back into introspection library
            if (symbol is IntrospectionTypeSymbol s)
                return module.GetType(s.QualifiedName).GetTypeInfo();

            return null;
        }

    }

}
