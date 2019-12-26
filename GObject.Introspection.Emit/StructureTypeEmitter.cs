using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

using GObject.Introspection.CodeGen.Model;

namespace GObject.Introspection.Emit
{

    class StructureTypeEmitter : TypeEmitter
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public StructureTypeEmitter(Context context) :
            base(context)
        {

        }

        protected override TypeAttributes GetTypeAttributes(Dynamic.Type type, bool isNestedType)
        {
            var t = (StructureType)type;

            var a = base.GetTypeAttributes(type, isNestedType) |
                TypeAttributes.Sealed |
                TypeAttributes.Serializable;

            if (t.LayoutKind == LayoutKind.Sequential)
                a |= TypeAttributes.SequentialLayout;

            if (t.LayoutKind == LayoutKind.Explicit)
                a |= TypeAttributes.ExplicitLayout;

            return a;
        }

        protected override TypeInfo GetParentType(Dynamic.Type type)
        {
            return typeof(ValueType).GetTypeInfo();
        }

        protected override void EmitCustomAttributes(TypeBuilder builder, Dynamic.Type type)
        {
            base.EmitCustomAttributes(builder, type);
            EmitStructLayoutAttribute(builder, (StructureType)type);
        }

        protected virtual void EmitStructLayoutAttribute(TypeBuilder builder, StructureType type)
        {
            if (type.LayoutKind != LayoutKind.Auto)
                builder.SetCustomAttribute(new CustomAttributeBuilder(
                    typeof(StructLayoutAttribute).GetConstructor(new[] { typeof(LayoutKind) }),
                    new object[] { type.LayoutKind }));
        }

    }

}
