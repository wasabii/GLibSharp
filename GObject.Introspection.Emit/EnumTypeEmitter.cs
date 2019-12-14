using System.Reflection;
using System.Reflection.Emit;

using GObject.Introspection.Reflection;

namespace GObject.Introspection.Dynamic
{

    /// <summary>
    /// Emits <see cref="EnumType"/>s to dynamic interop assemblies.
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

        protected override TypeAttributes GetTypeAttributes(IntrospectionType type, bool nested)
        {
            return base.GetTypeAttributes(type, nested) | TypeAttributes.Sealed;
        }

        protected override TypeInfo FinalizeDynamicType(TypeBuilder builder, IntrospectionType type)
        {
            var e = (EnumType)type;
            builder.DefineField("value__", Context.ResolveTypeInfo(e.BaseType), FieldAttributes.Private | FieldAttributes.SpecialName);
            return base.FinalizeDynamicType(builder, type);
        }

    }

}
