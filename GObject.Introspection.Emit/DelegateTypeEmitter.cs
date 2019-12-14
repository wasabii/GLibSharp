using System;
using System.Reflection;

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

        protected override TypeInfo GetParentType()
        {
            return typeof(MulticastDelegate).GetTypeInfo();
        }

    }

}
