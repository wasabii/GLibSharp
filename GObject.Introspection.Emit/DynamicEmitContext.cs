using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using GObject.Introspection.Reflection;

namespace GObject.Introspection.Dynamic
{

    /// <summary>
    /// Contextual information for generating dynamic types.
    /// </summary>
    public class DynamicEmitContext
    {

        readonly ModuleBuilder module;
        readonly TypeInfoResolver resolver;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="resolver"></param>
        public DynamicEmitContext(ModuleBuilder module, TypeInfoResolver resolver)
        {
            this.module = module ?? throw new ArgumentNullException(nameof(module));
            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
        }

        /// <summary>
        /// Gets the module being emitted.
        /// </summary>
        public ModuleBuilder Module => module;

        /// <summary>
        /// Disptachs the dynamic type emission to an emitter.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="nestedTypeParent"></param>
        /// <returns></returns>
        public IEnumerable<DynamicTypeInfo> EmitDynamicType(IntrospectionType type, TypeBuilder nestedTypeParent = null)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            switch (type)
            {
                case ClassType t:
                    return new ClassTypeEmitter(this).EmitDynamicType(t, nestedTypeParent);
                case StructureType t:
                    return new StructureTypeEmitter(this).EmitDynamicType(t, nestedTypeParent);
                case InterfaceType t:
                    return new InterfaceTypeEmitter(this).EmitDynamicType(t, nestedTypeParent);
                case EnumType t:
                    return new EnumTypeEmitter(this).EmitDynamicType(t, nestedTypeParent);
                case DelegateType t:
                    return new DelegateTypeEmitter(this).EmitDynamicType(t, nestedTypeParent);
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Disptachs the dynamic type emission to an emitter.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="nestedTypeParent"></param>
        /// <returns></returns>
        public IEnumerable<MemberInfo> EmitDynamicMember(TypeBuilder parent, IntrospectionMember member)
        {
            if (parent is null)
                throw new ArgumentNullException(nameof(parent));
            if (member is null)
                throw new ArgumentNullException(nameof(member));

            switch (member)
            {
                case FieldMember m:
                    return new FieldMemberEmitter(this).EmitDynamicMember(parent, m);
                case MethodMember m:
                    return new MethodMemberEmitter(this).EmitDynamicMember(parent, m);
                case PropertyMember m:
                    return new PropertyMemberEmitter(this).EmitDynamicMember(parent, m);
                case EventMember m:
                    return new EventMemberEmitter(this).EmitDynamicMember(parent, m);
                case EnumerationMember m:
                    return new EnumerationMemberEmitter(this).EmitDynamicMember(parent, m);
                case IntrospectionTypeMember m:
                    return Enumerable.Empty<MemberInfo>();
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Resolves the corresponding <see cref="TypeInfo"/> to refer to the specified <see cref="TypeSymbol"/>.
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public TypeInfo ResolveTypeInfo(TypeSymbol symbol)
        {
            if (symbol is null)
                throw new ArgumentNullException(nameof(symbol));

            return resolver.Resolve(symbol);
        }

    }

}
