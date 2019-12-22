using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace GObject.Introspection.Model
{

    /// <summary>
    /// Describes an introspected namespace.
    /// </summary>
    public class Module
    {

        /// <summary>
        /// Casts a list of type definitions to a list of types.
        /// </summary>
        class TypeList : IReadOnlyList<Type>
        {

            readonly Lazy<List<TypeDef>> defs;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="defs"></param>
            public TypeList(Func<List<TypeDef>> defs)
            {
                this.defs = new Lazy<List<TypeDef>>(defs);
            }

            public Type this[int index] => defs.Value[index]?.Type;

            public int Count => defs.Value.Count;

            public IEnumerator<Type> GetEnumerator()
            {
                return defs.Value.Select(i => i.Type).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

        }

        readonly Context context;
        readonly Xml.NamespaceElement ns;
        readonly Lazy<List<TypeDef>> typeDefs;
        readonly TypeList types;
        readonly ConcurrentDictionary<string, TypeDef> typeNameCache;
        readonly ConcurrentDictionary<string, TypeDef> managedTypeNameCache;
        readonly ConcurrentDictionary<string, TypeDef> nativeTypeNameCache;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        /// <param name="symbols"></param>
        /// <param name="ns"></param>
        internal Module(
            IManagedTypeResolver resolver,
            TypeSymbolProvider symbols,
            NativeTypeSymbolProvider nativeSymbols,
            Xml.NamespaceElement ns)
        {
            this.ns = ns ?? throw new ArgumentNullException(nameof(ns));

            var imports = GetImports(ns).ToList();
            context = new Context(this, resolver, symbols, nativeSymbols, imports, ns.Name);
            typeDefs = new Lazy<List<TypeDef>>(() => GetTypeDefs().ToList());
            types = new TypeList(() => typeDefs.Value);
            typeNameCache = new ConcurrentDictionary<string, TypeDef>();
            managedTypeNameCache = new ConcurrentDictionary<string, TypeDef>();
            nativeTypeNameCache = new ConcurrentDictionary<string, TypeDef>();
        }

        /// <summary>
        /// Gets the imports for the namesace.
        /// </summary>
        /// <param name="ns"></param>
        /// <returns></returns>
        IEnumerable<(string ns, string version)> GetImports(Xml.NamespaceElement ns)
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
        public IReadOnlyList<Type> Types => types;

        /// <summary>
        /// Gets the types within the namespace.
        /// </summary>
        /// <returns></returns>
        IEnumerable<TypeDef> GetTypeDefs()
        {
            return Enumerable.Empty<TypeDef>()
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

        IEnumerable<TypeDef> GetAliasTypes()
        {
            return ns.Aliases.Select(i => context.CreateType(i));
        }

        IEnumerable<TypeDef> GetBitFieldTypes()
        {
            return ns.BitFields.Select(i => context.CreateType(i));
        }

        IEnumerable<TypeDef> GetCallbackTypes()
        {
            return ns.Callbacks.Select(i => context.CreateType(i));
        }

        IEnumerable<TypeDef> GetClassTypes()
        {
            return ns.Classes.Select(i => context.CreateType(i));
        }

        IEnumerable<TypeDef> GetConstantTypes()
        {
            if (ns.Constants.Count > 0)
                yield return new TypeDef(Name + ".Constants", null, null, () => new ConstantClassType(context, ns.Constants));
        }

        IEnumerable<TypeDef> GetEnumerationTypes()
        {
            return ns.Enums.Select(i => context.CreateType(i));
        }

        IEnumerable<TypeDef> GetFunctionTypes()
        {
            if (ns.Functions.Count > 0)
                yield return new TypeDef(Name + ".Functions", null, null, () => new FunctionClassType(context, ns.Functions));
        }

        IEnumerable<TypeDef> GetInterfaceTypes()
        {
            return ns.Interfaces.Select(i => context.CreateType(i));
        }

        IEnumerable<TypeDef> GetRecordTypes()
        {
            return ns.Records.Select(i => context.CreateType(i));
        }

        IEnumerable<TypeDef> GetUnionTypes()
        {
            return ns.Unions.Select(i => context.CreateType(i));
        }

        /// <summary>
        /// Attempts to resolve the specified type from the namespace by the original introspected type name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal TypeDef ResolveTypeDef(string name)
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
        internal TypeDef ResolveTypeDefByManagedName(string name)
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
        internal TypeDef ResolveTypeDefByNativeName(string name)
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
        public Type ResolveType(string name)
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
        public Type ResolveTypeByManagedName(string name)
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
        public Type ResolveTypeByNativeName(string name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            return ResolveTypeDefByNativeName(name)?.Type;
        }

    }

}
