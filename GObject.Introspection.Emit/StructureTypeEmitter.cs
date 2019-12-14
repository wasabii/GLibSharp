using System;
using System.Reflection;

using GObject.Introspection.Reflection;

namespace GObject.Introspection.Dynamic
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

        protected override TypeAttributes GetTypeAttributes(IntrospectionType type, bool nested)
        {
            return base.GetTypeAttributes(type, nested) |
                TypeAttributes.Sealed |
                TypeAttributes.Serializable |
                TypeAttributes.AnsiClass;
        }

        protected override TypeInfo GetParentType(IntrospectionType type)
        {
            return typeof(ValueType).GetTypeInfo();
        }

    }

}
