﻿using System;
using System.Collections.Generic;
using System.Linq;

using GObject.Introspection.Model;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes a record that must be reflected as a reference type.
    /// </summary>
    class RecordElementStructureType : StructureType
    {

        readonly Record record;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="record"></param>
        public RecordElementStructureType(IntrospectionContext context, Record record) :
            base(context)
        {
            this.record = record ?? throw new ArgumentNullException(nameof(record));
        }

        /// <summary>
        /// Gets the name of the type.
        /// </summary>
        public override string Name => record.Name;

        /// <summary>
        /// Gets the original introspected name of the type.
        /// </summary>
        public override string IntrospectionName => record.Name;

        /// <summary>
        /// Gets the native name of the type.
        /// </summary>
        public override string NativeName => record.CType;

        /// <summary>
        /// Blittability is determined by whether the record is a struct or class implementation.
        /// </summary>
        /// <returns></returns>
        protected override bool GetIsBlittable()
        {
            return true;
        }

        /// <summary>
        /// Gets the members of the type.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<IntrospectionMember> GetMembers()
        {
            return base.GetMembers()
                .Concat(GetFieldCallbackTypes())
                .Concat(GetPropertyMembers())
                .Concat(GetFieldMembers())
                .Concat(GetConstructorMembers())
                .Concat(GetFunctionMembers())
                .Concat(GetMethodMembers())
                .Concat(GetUnionMembers());
        }

        /// <summary>
        /// Gets callback types declared as the type of fields.
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<IntrospectionMember> GetFieldCallbackTypes()
        {
            return record.Fields.Where(i => i.Callback != null).Select(i => GetFieldCallbackType(i));
        }

        protected virtual IntrospectionMember GetFieldCallbackType(Field field)
        {
            return new IntrospectionTypeMember(Context, this, new FieldElementMemberCallbackType(Context, this, field));
        }

        protected virtual IEnumerable<PropertyMember> GetPropertyMembers()
        {
            return record.Properties.Select(i => new PropertyElementMember(Context, this, i));
        }

        protected virtual IEnumerable<FieldMember> GetFieldMembers()
        {
            return record.Fields.Select(i => new FieldElementMember(Context, this, i));
        }

        protected virtual IEnumerable<MethodMember> GetConstructorMembers()
        {
            return record.Constructors.Select(i => new ConstructorElementMember(Context, this, i));
        }

        protected virtual IEnumerable<MethodMember> GetFunctionMembers()
        {
            return record.Functions.Select(i => new FunctionElementMember(Context, this, i));
        }

        protected virtual IEnumerable<MethodMember> GetMethodMembers()
        {
            return record.Methods.Select(i => new MethodElementMember(Context, this, i));
        }

        protected virtual IEnumerable<IntrospectionTypeMember> GetUnionMembers()
        {
            return record.Unions.Select(i => new IntrospectionTypeMember(Context, this, Context.CreateType(i).Type));
        }

    }

}