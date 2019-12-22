﻿using System;
using System.Reflection;
using System.Reflection.Emit;

using GObject.Introspection.Model;

namespace GObject.Introspection.Emit
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
            if (symbol is ModuleTypeSymbol s)
            {
                try
                {
                    var t = module.GetType(s.Name);
                    return t.GetTypeInfo();
                }
                catch (Exception e)
                {
                    throw;
                }
            }

            return null;
        }

    }

}
