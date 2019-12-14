using System;
using System.Reflection;
using System.Reflection.Emit;
using GObject.Introspection.Reflection;

namespace GObject.Introspection.Dynamic
{

    class DelegateTypeEmitter : TypeEmitter
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public DelegateTypeEmitter(DynamicEmitContext context) :
            base(context)
        {

        }

        protected override TypeInfo GetParentType(IntrospectionType type)
        {
            return typeof(MulticastDelegate).GetTypeInfo();
        }

        protected override TypeAttributes GetTypeAttributes(IntrospectionType type, bool isNestedType)
        {
            return base.GetTypeAttributes(type, isNestedType) | TypeAttributes.Sealed;
        }

        protected override TypeInfo FinalizeDynamicType(TypeBuilder builder, IntrospectionType type)
        {
            EmitDelegateMethods(builder, (DelegateType)type);
            return base.FinalizeDynamicType(builder, type);
        }

        void EmitDelegateMethods(TypeBuilder builder, DelegateType type)
        {
            var constructor = builder.DefineConstructor(
                MethodAttributes.RTSpecialName | MethodAttributes.HideBySig | MethodAttributes.Public,
                CallingConventions.Standard, new[] { typeof(object), typeof(IntPtr) });
            constructor.SetImplementationFlags(MethodImplAttributes.CodeTypeMask);

            var methodInvoke = builder.DefineMethod("Invoke",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual, CallingConventions.Standard,
                null,
                new Type[] { typeof(string) });
            methodInvoke.SetImplementationFlags(MethodImplAttributes.CodeTypeMask);
        }

    }

}
