using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes an introspected namespace.
    /// </summary>
    public class IntrospectionModule
    {

        /// <summary>
        /// Casts a list of type definitions to a list of types.
        /// </summary>
        class TypeList : IReadOnlyList<IntrospectionType>
        {

            readonly Lazy<List<IntrospectionTypeDef>> defs;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="defs"></param>
            public TypeList(Func<List<IntrospectionTypeDef>> defs)
            {
                this.defs = new Lazy<List<IntrospectionTypeDef>>(defs);
            }

            public IntrospectionType this[int index] => defs.Value[index]?.Type;

            public int Count => defs.Value.Count;

            public IEnumerator<IntrospectionType> GetEnumerator()
            {
                return defs.Value.Select(i => i.Type).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

        }

        readonly IntrospectionContext context;
        readonly Model.Namespace ns;
        readonly Lazy<List<IntrospectionTypeDef>> typeDefs;
        readonly TypeList types;
        readonly ConcurrentDictionary<string, IntrospectionTypeDef> typeNameCache;
        readonly ConcurrentDictionary<string, IntrospectionTypeDef> managedTypeNameCache;
        readonly ConcurrentDictionary<string, IntrospectionTypeDef> nativeTypeNameCache;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        /// <param name="symbols"></param>
        /// <param name="ns"></param>
        internal IntrospectionModule(
            IManagedTypeResolver resolver,
            TypeSymbolProvider symbols,
            NativeTypeSymbolProvider nativeSymbols,
            Model.Namespace ns)
        {
            this.ns = ns ?? throw new ArgumentNullException(nameof(ns));

            var imports = GetImports(ns).ToList();
            context = new IntrospectionContext(this, resolver, symbols, nativeSymbols, imports, ns.Name);
            typeDefs = new Lazy<List<IntrospectionTypeDef>>(() => GetTypeDefs().ToList());
            types = new TypeList(() => typeDefs.Value);
            typeNameCache = new ConcurrentDictionary<string, IntrospectionTypeDef>();
            managedTypeNameCache = new ConcurrentDictionary<string, IntrospectionTypeDef>();
            nativeTypeNameCache = new ConcurrentDictionary<string, IntrospectionTypeDef>();
        }

        /// <summary>
        /// Gets the imports for the namesace.
        /// </summary>
        /// <param name="ns"></param>
        /// <returns></returns>
        IEnumerable<(string ns, string version)> GetImports(Model.Namespace ns)
        {
            foreach (var i in ns.Repository.Includes)
                yield return (i.Name, i.Version);

            yield return (ns.Name, ns.Version);
            yield return ("C", "1.0");
        }

        /// <summary>
        /// Gets the name of the namespace.
        /// </summary>
        public string Name => ns.Name;

        /// <summary>
        /// Gets the original introspected name of the namespace.
        /// </summary>
        public string IntrospectionName => ns.Name;

        /// <summary>
        /// Gets the types within the namespace.
        /// </summary>
        public IReadOnlyList<IntrospectionType> Types => types;

        /// <summary>
        /// Gets the types within the namespace.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IntrospectionTypeDef> GetTypeDefs()
        {
            return Enumerable.Empty<IntrospectionTypeDef>()
                .Concat(GetAliasTypes())
                .Concat(GetBitFieldTypes())
                .Concat(GetCallbackTypes())
                .Concat(GetClassTypes())
                .Concat(GetConstantTypes())
                .Concat(GetEnumerationTypes())
                .Concat(GetFunctionTypes())
                .Concat(GetInterfaceTypes())
                .Concat(GetRecordTypes())
                .Concat(GetUnionTypes());
        }

        IEnumerable<IntrospectionTypeDef> GetAliasTypes()
        {
            return ns.Aliases.Select(i => context.CreateType(i));
        }

        IEnumerable<IntrospectionTypeDef> GetBitFieldTypes()
        {
            return ns.BitFields.Select(i => context.CreateType(i));
        }

        IEnumerable<IntrospectionTypeDef> GetCallbackTypes()
        {
            return ns.Callbacks.Select(i => context.CreateType(i));
        }

        IEnumerable<IntrospectionTypeDef> GetClassTypes()
        {
            return ns.Classes.Select(i => context.CreateType(i));
        }

        IEnumerable<IntrospectionTypeDef> GetConstantTypes()
        {
            if (ns.Constants.Count > 0)
                yield return new IntrospectionTypeDef(Name + ".Constants", null, null, () => new ConstantClassType(context, ns.Constants));
        }

        IEnumerable<IntrospectionTypeDef> GetEnumerationTypes()
        {
            return ns.Enums.Select(i => context.CreateType(i));
        }

        IEnumerable<IntrospectionTypeDef> GetFunctionTypes()
        {
            if (ns.Functions.Count > 0)
                yield return new IntrospectionTypeDef(Name + ".Functions", null, null, () => new FunctionClassType(context, ns.Functions));
        }

        IEnumerable<IntrospectionTypeDef> GetInterfaceTypes()
        {
            return ns.Interfaces.Select(i => context.CreateType(i));
        }

        IEnumerable<IntrospectionTypeDef> GetRecordTypes()
        {
            return ns.Records.Select(i => context.CreateType(i));
        }

        IEnumerable<IntrospectionTypeDef> GetUnionTypes()
        {
            return ns.Unions.Select(i => context.CreateType(i));
        }

        /// <summary>
        /// Attempts to resolve the specified type from the namespace by the original introspected type name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal IntrospectionTypeDef ResolveTypeDef(string name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            return typeNameCache.GetOrAdd(name, typeDefs.Value.Where(i => i.IntrospectionName == name).FirstOrDefault());
        }

        /// <summary>
        /// Attempts to resolve the specified type from the namespace by the managed type name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal IntrospectionTypeDef ResolveTypeDefByManagedName(string name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            return managedTypeNameCache.GetOrAdd(name, typeDefs.Value.Where(i => i.Name == name).FirstOrDefault());
        }

        /// <summary>
        /// Attempts to resolve the specified type from the namespace by the managed type name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal IntrospectionTypeDef ResolveTypeDefByNativeName(string name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            return nativeTypeNameCache.GetOrAdd(name, typeDefs.Value.Where(i => i.NativeName == name).FirstOrDefault());
        }

        /// <summary>
        /// Attempts to resolve the specified type name from the namespace by the original introspected type name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IntrospectionType ResolveType(string name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            return ResolveTypeDef(name);
        }

        /// <summary>
        /// Attempts to resolve the specified type name from the namespace.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IntrospectionType ResolveTypeByManagedName(string name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            return ResolveTypeDefByManagedName(name)?.Type;
        }

        /// <summary>
        /// Attempts to resolve the specified native type name from the namespace.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IntrospectionType ResolveTypeByNativeName(string name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            return ResolveTypeDefByNativeName(name)?.Type;
        }

    }

}
