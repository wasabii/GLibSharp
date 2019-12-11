using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes a type within the introspectable namespace.
    /// </summary>
    public abstract class IntrospectionType
    {

        readonly Lazy<List<TypeSymbol>> baseTypes;
        readonly Lazy<List<IntrospectionMember>> members;
        readonly ConcurrentDictionary<string, IntrospectionMember> memberNameCache;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public IntrospectionType(IntrospectionContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));

            baseTypes = new Lazy<List<TypeSymbol>>(() => GetBaseTypes().ToList());
            members = new Lazy<List<IntrospectionMember>>(() => GetMembers().ToList());
            memberNameCache = new ConcurrentDictionary<string, IntrospectionMember>();
        }

        /// <summary>
        /// Gets the current introspection context of the type.
        /// </summary>
        public IntrospectionContext Context { get; }

        /// <summary>
        /// Gets the original name of the type.
        /// </summary>
        public abstract string IntrospectionName { get; }

        /// <summary>
        /// Gets the name of the type within its namespace.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the qualified name of the type.
        /// </summary>
        public string QualifiedName => Context.CurrentNamespace + "." + Name;

        /// <summary>
        /// Gets the type of the type.
        /// </summary>
        public abstract IntrospectionTypeKind Kind { get; }

        /// <summary>
        /// Gets the visibility of the type.
        /// </summary>
        public virtual IntrospectionVisibility Visibility => IntrospectionVisibility.Public;

        /// <summary>
        /// Gets the base types of the type.
        /// </summary>
        public virtual IReadOnlyList<TypeSymbol> BaseTypes => baseTypes.Value;

        /// <summary>
        /// Gets the members of the type.
        /// </summary>
        public virtual IReadOnlyList<IntrospectionMember> Members => members.Value;

        /// <summary>
        /// Gets the base types of the type.
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<TypeSymbol> GetBaseTypes()
        {
            return Enumerable.Empty<TypeSymbol>();
        }

        /// <summary>
        /// Gets the members of the type.
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<IntrospectionMember> GetMembers()
        {
            return Enumerable.Empty<IntrospectionMember>();
        }

        /// <summary>
        /// Attempts to resolve the type member with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IntrospectionMember ResolveMember(string name)
        {
            return memberNameCache.GetOrAdd(name, Members.FirstOrDefault(i => i.Name == name));
        }

    }

}
