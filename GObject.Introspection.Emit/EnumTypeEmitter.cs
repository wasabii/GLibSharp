using System;
using System.Reflection;
using System.Reflection.Emit;

using GObject.Introspection.Reflection;

namespace GObject.Introspection.Dynamic
{

    /// <summary>
    /// Emits <see cref="FlagElementType"/>s to dynamic interop assemblies.
    /// </summary>
    class EnumTypeEmitter : TypeEmitter
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public EnumTypeEmitter(DynamicEmitContext context) :
            base(context)
        {

        }

        protected override TypeInfo GetParentType(IntrospectionType type)
        {
            return typeof(Enum).GetTypeInfo();
        }

        protected override TypeAttributes GetTypeAttributes(IntrospectionType type, bool nested)
        {
            return base.GetTypeAttributes(type, nested) | TypeAttributes.Sealed;
        }

        protected override TypeInfo FinalizeDynamicType(TypeBuilder builder, IntrospectionType type)
        {
            return FinalizeDynamicType(builder, (FlagElementType)type);
        }

        TypeInfo FinalizeDynamicType(TypeBuilder builder, FlagElementType type)
        {
            builder.DefineField("value__", typeof(int), FieldAttributes.Private | FieldAttributes.SpecialName);
            return base.FinalizeDynamicType(builder, type);
        }

    }

}
