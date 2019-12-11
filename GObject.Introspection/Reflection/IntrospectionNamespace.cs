using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes a managed namespace derived from an introspected namespace.
    /// </summary>
    public class IntrospectionNamespace
    {

        readonly IntrospectionContext context;
        readonly Model.Namespace ns;
        readonly Lazy<List<IntrospectionType>> types;
        readonly ConcurrentDictionary<string, IntrospectionType> typeNameCache;
        readonly ConcurrentDictionary<string, IntrospectionType> introspectedTypeNameCache;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ns"></param>
        public IntrospectionNamespace(IntrospectionContext context, Model.Namespace ns)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.ns = ns ?? throw new ArgumentNullException(nameof(ns));

            types = new Lazy<List<IntrospectionType>>(() => GetTypes().ToList());
            typeNameCache = new ConcurrentDictionary<string, IntrospectionType>();
            introspectedTypeNameCache = new ConcurrentDictionary<string, IntrospectionType>();
        }

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

        protected virtual IEnumerable<IntrospectionType> GetBitFieldTypes()
        {
            return ns.BitFields.Select(i => new BitFieldType(context, i));
        }

        protected virtual IEnumerable<IntrospectionType> GetCallbackTypes()
        {
            return ns.Callbacks.Select(i => new CallbackType(context, i));
        }

        protected virtual IEnumerable<IntrospectionType> GetClassTypes()
        {
            return ns.Classes.Select(i => new ClassType(context, i));
        }

        protected virtual IEnumerable<IntrospectionType> GetConstantTypes()
        {
            if (ns.Constants.Count > 0)
                yield return new ConstantType(context, ns.Constants);
        }

        protected virtual IEnumerable<IntrospectionType> GetEnumerationTypes()
        {
            return ns.Enums.Select(i => new EnumerationType(context, i));
        }

        protected virtual IEnumerable<IntrospectionType> GetFunctionTypes()
        {
            if (ns.Functions.Count > 0)
                yield return new FunctionType(context, ns.Functions);
        }

        protected virtual IEnumerable<IntrospectionType> GetInterfaceTypes()
        {
            return ns.Interfaces.Select(i => new InterfaceType(context, i));
        }

        protected virtual IEnumerable<IntrospectionType> GetRecordTypes()
        {
            return ns.Records.Select(i => new RecordType(context, i));
        }

        protected virtual IEnumerable<IntrospectionType> GetUnionTypes()
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
