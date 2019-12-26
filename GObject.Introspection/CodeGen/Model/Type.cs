using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace GObject.Introspection.CodeGen.Model
{

    /// <summary>
    /// Describes a type within the introspectable namespace.
    /// </summary>
    abstract class Type
    {

        readonly Context context;
        readonly Lazy<bool> isBlittable;
        readonly Lazy<ITypeSymbol> baseType;
        readonly Lazy<List<ITypeSymbol>> implementedInterfaces;
        readonly Lazy<List<Member>> members;
        readonly ConcurrentDictionary<string, Member> memberNameCache;
        readonly Lazy<List<Attribute>> attributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        internal Type(Context context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));

            isBlittable = new Lazy<bool>(GetIsBlittable);
            baseType = new Lazy<ITypeSymbol>(GetBaseType);
            implementedInterfaces = new Lazy<List<ITypeSymbol>>(() => GetImplementedInterfaces().ToList());
            members = new Lazy<List<Member>>(() => GetMembers().ToList());
            memberNameCache = new ConcurrentDictionary<string, Member>();
            attributes = new Lazy<List<Attribute>>(() => GetAttributes().ToList());
        }

        /// <summary>
        /// Gets the current introspection context of the type.
        /// </summary>
        internal Context Context => context;

        /// <summary>
        /// Gets the namespace of the type.
        /// </summary>
        public Module Module => context.Module;

        /// <summary>
        /// Gets the name of the type within its namespace.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the original name of the type.
        /// </summary>
        public abstract string IntrospectionName { get; }

        /// <summary>
        /// Gets the native name of the type.
        /// </summary>
        public abstract string NativeName { get; }

        /// <summary>
        /// Gets the qualified name of the type.
        /// </summary>
        public virtual string QualifiedName => Context.CurrentNamespace + "." + Name;

        /// <summary>
        /// Does the type represent a value which is blittable to the corresponding native type.
        /// </summary>
        public bool IsBlittable => isBlittable.Value;

        /// <summary>
        /// Returns whether or not the type is blittable.
        /// </summary>
        /// <returns></returns>
        protected virtual bool GetIsBlittable()
        {
            switch (this)
            {
                case ClassType c:
                case DelegateType d:
                case InterfaceType i:
                    return false;
                case StructureType s:
                    return Members.OfType<FieldMember>().All(i => i.FieldType.IsBlittable);
                case EnumType e:
                    return true;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets the visibility of the type.
        /// </summary>
        public virtual Visibility Visibility => Visibility.Public;

        /// <summary>
        /// Gets the modifiers applied to the type.
        /// </summary>
        public virtual Modifier Modifiers => Modifier.Default;

        /// <summary>
        /// Gets the base types of the type.
        /// </summary>
        public virtual ITypeSymbol BaseType => baseType.Value;

        /// <summary>
        /// Gets the base types of the type.
        /// </summary>
        /// <returns></returns>
        protected virtual ITypeSymbol GetBaseType()
        {
            return null;
        }

        /// <summary>
        /// Gets the base types of the type.
        /// </summary>
        public virtual IReadOnlyList<ITypeSymbol> ImplementedInterfaces => implementedInterfaces.Value;

        /// <summary>
        /// Gets the base types of the type.
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<ITypeSymbol> GetImplementedInterfaces()
        {
            return Enumerable.Empty<ITypeSymbol>();
        }

        /// <summary>
        /// Gets the members of the type.
        /// </summary>
        public virtual IReadOnlyList<Member> Members => members.Value;

        /// <summary>
        /// Gets the members of the type.
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<Member> GetMembers()
        {
            return Enumerable.Empty<Member>();
        }

        /// <summary>
        /// Attempts to resolve the type member with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Member ResolveMember(string name)
        {
            return memberNameCache.GetOrAdd(name, Members.FirstOrDefault(i => i.Name == name));
        }

        /// <summary>
        /// Gets the attributes to be applied to the type.
        /// </summary>
        public IReadOnlyList<Attribute> Attributes => attributes.Value;

        /// <summary>
        /// Gets the attributes of the type.
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<Attribute> GetAttributes()
        {
            yield break;
        }

        /// <summary>
        /// Returns a string representation of the type.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }

    }

}
