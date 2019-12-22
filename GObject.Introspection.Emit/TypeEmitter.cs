﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using GObject.Introspection.Model;

namespace GObject.Introspection.Emit
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
        protected virtual TypeAttributes GetTypeAttributes(Model.Type type, bool isNestedType)
        {
            var a = TypeAttributes.AnsiClass;

            switch (type.Visibility)
            {
                case Visibility.Public:
                    a |= isNestedType == false ? TypeAttributes.Public : TypeAttributes.NestedPublic;
                    break;
                case Visibility.Private:
                    a |= isNestedType == false ? TypeAttributes.NotPublic : TypeAttributes.NestedPrivate;
                    break;
                case Visibility.Internal:
                    break;
            }

            return a;
        }

        /// <summary>
        /// Gets the parent type to apply on generation.
        /// </summary>
        /// <returns></returns>
        protected virtual TypeInfo GetParentType(Model.Type type)
        {
            return type.BaseType != null ? Context.ResolveTypeInfo(type.BaseType) : null;
        }

        /// <summary>
        /// Emits a set of dynamic type info structures to generate the dynamic types for the introspection type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="nestedTypeParent"></param>
        /// <returns></returns>
        public virtual IEnumerable<DynamicTypeInfo> EmitDynamicType(Model.Type type, TypeBuilder nestedTypeParent)
        {
            // build the new type definition
            var builder = DefineType(type, GetTypeAttributes(type, nestedTypeParent != null), GetParentType(type), nestedTypeParent);
            yield return DynamicTypeInfo.Create(type, builder, t => FinalizeDynamicType(builder, t));

            // emit nested type members
            foreach (var nested in type.Members.OfType<TypeMember>())
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
        protected virtual TypeBuilder DefineType(Model.Type type, TypeAttributes attr, System.Type parent, TypeBuilder nestedTypeParent)
        {
            if (nestedTypeParent == null)
            {
                var n = type.Name;
                Console.WriteLine("Emitting {0}", type.Name);

                // standard type, at the module level
                return Context.Module.DefineType(type.Module.Name + "." + type.Name, attr, parent);
            }
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
        protected virtual TypeInfo FinalizeDynamicType(TypeBuilder builder, Model.Type type)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            EmitCustomAttributes(builder, type);
            EmitMembers(builder, type);

            return builder.CreateTypeInfo();
        }

        /// <summary>
        /// Emits custom attributes on the type.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type"></param>
        protected virtual void EmitCustomAttributes(TypeBuilder builder, Model.Type type)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));
            if (type is null)
                throw new ArgumentNullException(nameof(type));
        }

        /// <summary>
        /// Emits the members of the type.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type"></param>
        protected virtual void EmitMembers(TypeBuilder builder, Model.Type type)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            foreach (var member in type.Members)
                foreach (var info in Context.EmitDynamicMember(builder, member))
                    info.GetHashCode();
        }

    }

}
