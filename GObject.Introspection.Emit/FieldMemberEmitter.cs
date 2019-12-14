using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

using GObject.Introspection.Reflection;

namespace GObject.Introspection.Dynamic
{

    class FieldMemberEmitter : MemberEmitter
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public FieldMemberEmitter(DynamicEmitContext context) :
            base(context)
        {

        }

        protected virtual FieldAttributes GetFieldAttributes(TypeBuilder type, FieldMember field)
        {
            var a = FieldAttributes.PrivateScope;

            switch (field.Visibility)
            {
                case IntrospectionVisibility.Public:
                    a |= FieldAttributes.Public;
                    break;
                case IntrospectionVisibility.Private:
                    a |= FieldAttributes.Private;
                    break;
                case IntrospectionVisibility.Internal:
                    a |= FieldAttributes.Family;
                    break;
            }

            return a;
        }

        public override IEnumerable<MemberInfo> EmitDynamicMember(TypeBuilder type, IntrospectionMember member)
        {
            return EmitDynamicMember(type, (FieldMember)member);
        }

        IEnumerable<MemberInfo> EmitDynamicMember(TypeBuilder type, FieldMember field)
        {
            var fieldType = Context.ResolveTypeInfo(field.FieldType);
            if (fieldType is null)
                throw new InvalidOperationException("Could not resolve field type.");

            var builder = type.DefineField(field.Name, fieldType, GetFieldAttributes(type, field));
            yield return builder;
        }

    }

}
