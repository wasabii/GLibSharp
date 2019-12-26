using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

using GObject.Introspection.CodeGen.Model;

namespace GObject.Introspection.Emit
{

    class FieldMemberEmitter : MemberEmitter
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public FieldMemberEmitter(Context context) :
            base(context)
        {

        }

        FieldAttributes GetFieldAttributes(TypeBuilder type, FieldMember field)
        {
            var a = FieldAttributes.PrivateScope;

            switch (field.Visibility)
            {
                case Visibility.Public:
                    a |= FieldAttributes.Public;
                    break;
                case Visibility.Private:
                    a |= FieldAttributes.Private;
                    break;
                case Visibility.Internal:
                    a |= FieldAttributes.Family;
                    break;
            }

            return a;
        }

        public override IEnumerable<MemberInfo> EmitDynamicMember(TypeBuilder type, Member member)
        {
            return EmitDynamicMember(type, (FieldMember)member);
        }

        /// <summary>
        /// Emits the member information for the field.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        IEnumerable<MemberInfo> EmitDynamicMember(TypeBuilder type, FieldMember field)
        {
            var fieldType = Context.ResolveTypeInfo(field.FieldType.Type);
            if (fieldType is null)
                throw new InvalidOperationException("Could not resolve field type.");

            // field is a fixed array
            if (field.FieldType is ArrayTypeSpec array && array.FixedSize != null)
            {
                foreach (var i in EmitFixedArrayField(type, field, array))
                    yield return i;

                yield break;
            }

            var fieldBuilder = type.DefineField(field.Name, fieldType, GetFieldAttributes(type, field));

            // set explicit offset if required
            if (field.Offset != null)
                fieldBuilder.SetOffset((int)field.Offset);

            yield return fieldBuilder;
        }

        /// <summary>
        /// Emits the members required to implement a fixed array field.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="field"></param>
        /// <param name="typeSpec"></param>
        /// <returns></returns>
        IEnumerable<MemberInfo> EmitFixedArrayField(TypeBuilder type, FieldMember field, ArrayTypeSpec typeSpec)
        {
            var baseType = Context.ResolveTypeInfo(typeSpec.ItemType.Type);
            if (baseType == null)
                throw new InvalidOperationException("Could not resolve item type of fixed array.");

            // nested type for the fixed buffer
            var fixedTypeBuilder = type.DefineNestedType(
                $"<{field.Name}>e__FixedBuffer",
                    TypeAttributes.NestedPublic |
                    TypeAttributes.SequentialLayout |
                    TypeAttributes.AnsiClass |
                    TypeAttributes.Sealed |
                    TypeAttributes.BeforeFieldInit,
                typeof(ValueType));
            fixedTypeBuilder.SetCustomAttribute(new CustomAttributeBuilder(typeof(CompilerGeneratedAttribute).GetConstructor(new System.Type[0]), new object[0]));
            fixedTypeBuilder.SetCustomAttribute(new CustomAttributeBuilder(typeof(UnsafeValueTypeAttribute).GetConstructor(new System.Type[0]), new object[0]));
            fixedTypeBuilder.DefineField("FixedElementField", baseType, FieldAttributes.Public);
            fixedTypeBuilder.CreateTypeInfo();

            // field is typed as the nested type
            var fieldBuilder = type.DefineField(field.Name, fixedTypeBuilder, GetFieldAttributes(type, field));
            fieldBuilder.SetCustomAttribute(new CustomAttributeBuilder(typeof(FixedBufferAttribute).GetConstructor(new[] { typeof(System.Type), typeof(Int32) }), new object[] { baseType, typeSpec.FixedSize }));

            // set explicit offset if required
            if (field.Offset != null)
                fieldBuilder.SetOffset((int)field.Offset);

            yield return fieldBuilder;
        }

    }

}
