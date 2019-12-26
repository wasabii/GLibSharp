using System;
using System.Reflection;
using System.Reflection.Emit;

using GObject.Introspection.CodeGen.Model;

namespace GObject.Introspection.Emit
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
        public EnumTypeEmitter(Context context) :
            base(context)
        {

        }

        protected override TypeInfo GetParentType(Dynamic.Type type)
        {
            return typeof(Enum).GetTypeInfo();
        }

        protected override TypeAttributes GetTypeAttributes(Dynamic.Type type, bool nested)
        {
            return base.GetTypeAttributes(type, nested) | TypeAttributes.Sealed;
        }

        protected override TypeInfo FinalizeDynamicType(TypeBuilder builder, Dynamic.Type type)
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
