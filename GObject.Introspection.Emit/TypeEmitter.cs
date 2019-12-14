using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using GObject.Introspection.Reflection;

namespace GObject.Introspection.Dynamic
{

    /// <summary>
    /// Base type for classes that emit interop types.
    /// </summary>
    abstract class TypeEmitter
    {

        readonly DynamicEmitContext context;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public TypeEmitter(DynamicEmitContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Gets the context of the emission process.
        /// </summary>
        public DynamicEmitContext Context => context;

        /// <summary>
        /// Gets the <see cref="TypeAttributes"/> to apply on generation.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="isNestedType"></param>
        /// <returns></returns>
        protected virtual TypeAttributes GetTypeAttributes(IntrospectionType type, bool isNestedType)
        {
            var a = TypeAttributes.AnsiClass;

            switch (type.Visibility)
            {
                case IntrospectionVisibility.Public:
                    a |= isNestedType == false ? TypeAttributes.Public : TypeAttributes.NestedPublic;
                    break;
                case IntrospectionVisibility.Private:
                    a |= isNestedType == false ? TypeAttributes.NotPublic : TypeAttributes.NestedPrivate;
                    break;
                case IntrospectionVisibility.Internal:
                    break;
            }

            return a;
        }

        /// <summary>
        /// Gets the parent type to apply on generation.
        /// </summary>
        /// <returns></returns>
        protected virtual TypeInfo GetParentType()
        {
            return typeof(object).GetTypeInfo();
        }

        /// <summary>
        /// Emits a set of dynamic type info structures to generate the dynamic types for the introspection type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="nestedTypeParent"></param>
        /// <returns></returns>
        public virtual IEnumerable<DynamicTypeInfo> EmitDynamicType(IntrospectionType type, TypeBuilder nestedTypeParent)
        {
            // build the new type definition
            var builder = DefineType(type, GetTypeAttributes(type, nestedTypeParent != null), GetParentType(), nestedTypeParent);
            yield return DynamicTypeInfo.Create(type, builder, t => FinalizeDynamicType(builder, t));

            // emit nested type members
            foreach (var nested in type.Members.OfType<IntrospectionTypeMember>())
                foreach (var info in Context.EmitDynamicType(nested.Type, builder))
                    yield return info;
        }

        /// <summary>
        /// Defines the type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="attr"></param>
        /// <param name="parent"></param>
        /// <param name="nestedTypeParent"></param>
        /// <returns></returns>
        protected virtual TypeBuilder DefineType(IntrospectionType type, TypeAttributes attr, Type parent, TypeBuilder nestedTypeParent)
        {
            if (nestedTypeParent == null)
                // standard type, at the module level
                return Context.Module.DefineType(type.Module.Name + "." + type.Name, attr, parent);
            else
                // nested type, within a parent type
                return nestedTypeParent.DefineNestedType(type.Name, attr, parent);
        }

        /// <summary>
        /// Second phase, finalizes the type builder.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected virtual TypeInfo FinalizeDynamicType(TypeBuilder builder, IntrospectionType type)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            // emit members of the type
            EmitMembers(builder, type);

            return builder.CreateTypeInfo();
        }

        /// <summary>
        /// Emits the members of the type.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type"></param>
        protected virtual void EmitMembers(TypeBuilder builder, IntrospectionType type)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            foreach (var member in type.Members)
                EmitMember(builder, type, member);
        }

        /// <summary>
        /// Emits the specified type member.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type"></param>
        /// <param name="member"></param>
        protected virtual void EmitMember(TypeBuilder builder, IntrospectionType type, IntrospectionMember member)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            if (member is null)
                throw new ArgumentNullException(nameof(member));

            switch (member)
            {
                case FieldMember field:
                    EmitMember(builder, type, field);
                    break;
                case MethodMember method:
                    EmitMember(builder, type, method);
                    break;
                case PropertyMember property:
                    EmitMember(builder, type, property);
                    break;
                case EventMember evnt:
                    EmitMember(builder, type, evnt);
                    break;
                case EnumerationMember field:
                    EmitMember(builder, type, field);
                    break;
            }
        }

        /// <summary>
        /// Emits field members.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type"></param>
        /// <param name="field"></param>
        protected void EmitMember(TypeBuilder builder, IntrospectionType type, FieldMember field)
        {
            var f = builder.DefineField(
                field.Name,
                Context.ResolveTypeInfo(field.FieldType),
                FieldAttributes.Public);
        }

        /// <summary>
        /// Emits method members.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type"></param>
        /// <param name="method"></param>
        protected void EmitMember(TypeBuilder builder, IntrospectionType type, MethodMember method)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Emits property members.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type"></param>
        /// <param name="property"></param>
        protected void EmitMember(TypeBuilder builder, IntrospectionType type, PropertyMember property)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Emits event members.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type"></param>
        /// <param name="evnt"></param>
        protected void EmitMember(TypeBuilder builder, IntrospectionType type, EventMember evnt)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Emits enum members.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type"></param>
        /// <param name="field"></param>
        protected void EmitMember(TypeBuilder builder, IntrospectionType type, EnumerationMember field)
        {
            var f = builder.DefineField(field.Name, builder, FieldAttributes.Public | FieldAttributes.Literal | FieldAttributes.Static);
            f.SetConstant(field.Value);
        }

    }

}
