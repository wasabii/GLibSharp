using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using GObject.Introspection.Model;

namespace GObject.Introspection.Emit
{

    /// <summary>
    /// Provides a source of <see cref="TypeInfo"/> instances across a set of loaded assemblies.
    /// </summary>
    public class AssemblyTypeInfoSource : ITypeInfoSource
    {

        readonly IEnumerable<Assembly> assemblies;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="assemblies"></param>
        public AssemblyTypeInfoSource(IEnumerable<Assembly> assemblies)
        {
            this.assemblies = assemblies ?? throw new ArgumentNullException(nameof(assemblies));
        }

        public TypeInfo ResolveTypeInfo(TypeSymbol symbol)
        {
            return assemblies.Select(i => i.GetType(symbol.Name)?.GetTypeInfo()).FirstOrDefault(i => i != null);
        }

    }

}