using System;
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

        readonly IntrospectionContext context;
        readonly Model.Namespace ns;
        readonly Lazy<List<IntrospectionType>> types;
        readonly ConcurrentDictionary<string, IntrospectionType> typeNameCache;
        readonly ConcurrentDictionary<string, IntrospectionType> introspectedTypeNameCache;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="symbols"></param>
        /// <param name="ns"></param>
        public IntrospectionModule(TypeSymbolProvider symbols, Model.Namespace ns)
        {
            this.ns = ns ?? throw new ArgumentNullException(nameof(ns));

            var imports = ns.Repository.Includes.Select(i => (i.Name, i.Version)).Append((ns.Name, ns.Version)).ToList();
            context = new IntrospectionContext(this, symbols, imports, ns.Name);
            types = new Lazy<List<IntrospectionType>>(() => GetTypes().ToList());
            typeNameCache = new ConcurrentDictionary<string, IntrospectionType>();
            introspectedTypeNameCache = new ConcurrentDictionary<string, IntrospectionType>();
        }

        /// <summary>
        /// Gets the name of the namespace.
        /// </summary>
        public string Name => ns.Name;

        /// <summary>
        /// Gets the types within the namespace.
        /// </summary>
        public IReadOnlyList<IntrospectionType> Types => types.Value;

        /// <summary>
        /// Gets the types within the namespace.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IntrospectionType> GetTypes()
        {
            return Enumerable.Empty<IntrospectionType>()
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

        protected virtual IEnumerable<IntrospectionType> GetAliasTypes()
        {
            return ns.Aliases.Select(i => new AliasElementType(context, i));
        }

        protected virtual IEnumerable<EnumType> GetBitFieldTypes()
        {
            return ns.BitFields.Select(i => new BitFieldElementType(context, i));
        }

        protected virtual IEnumerable<DelegateType> GetCallbackTypes()
        {
            return ns.Callbacks.Select(i => new CallbackElementType(context, i));
        }

        protected virtual IEnumerable<ClassType> GetClassTypes()
        {
            return ns.Classes.Select(i => new ClassElementType(context, i));
        }

        protected virtual IEnumerable<ClassType> GetConstantTypes()
        {
            if (ns.Constants.Count > 0)
                yield return new ConstantClassType(context, ns.Constants);
        }

        protected virtual IEnumerable<EnumType> GetEnumerationTypes()
        {
            return ns.Enums.Select(i => new EnumerationElementType(context, i));
        }

        protected virtual IEnumerable<ClassType> GetFunctionTypes()
        {
            if (ns.Functions.Count > 0)
                yield return new FunctionClassType(context, ns.Functions);
        }

        protected virtual IEnumerable<InterfaceType> GetInterfaceTypes()
        {
            return ns.Interfaces.Select(i => new InterfaceElementType(context, i));
        }

        protected virtual IEnumerable<ClassType> GetRecordTypes()
        {
            return ns.Records.Select(i => new RecordElementType(context, i));
        }

        protected virtual IEnumerable<StructureType> GetUnionTypes()
        {
            return ns.Unions.Select(i => new UnionType(context, i));
        }

        /// <summary>
        /// Attempts to resolve the specified type name from the namespace.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IntrospectionType ResolveType(string name)
        {
            return typeNameCache.GetOrAdd(name, Types.FirstOrDefault(i => i.Name == name));
        }

        /// <summary>
        /// Attempts to resolve the specified type name from the namespace by the original introspected type name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IntrospectionType ResolveTypeByIntrospectionName(string name)
        {
            return introspectedTypeNameCache.GetOrAdd(name, Types.FirstOrDefault(i => i.IntrospectionName == name));
        }

    }

}
