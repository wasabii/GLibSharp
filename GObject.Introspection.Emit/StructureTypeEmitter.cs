using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

using GObject.Introspection.Model;

namespace GObject.Introspection.Emit
{

    class StructureTypeEmitter : TypeEmitter
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public StructureTypeEmitter(DynamicEmitContext context) :
            base(context)
        {

        }

        protected override TypeAttributes GetTypeAttributes(Model.Type type, bool isNestedType)
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

        protected override TypeInfo GetParentType(Model.Type type)
        {
            return typeof(ValueType).GetTypeInfo();
        }

        protected override void EmitCustomAttributes(TypeBuilder builder, Model.Type type)
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
