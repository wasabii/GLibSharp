using System;

namespace GObject.Introspection.Reflection
{

    class HandleFieldMember : FieldMember
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        public HandleFieldMember(IntrospectionContext context, IntrospectionType declaringType) :
            base(context, declaringType)
        {

        }

        public override string Name => "_handle";

        protected override TypeSpec GetFieldType()
        {
            return new TypeSpec(Context.ResolveManagedSymbol(typeof(IntPtr).FullName), Context.ResolveNativeSymbol("void*"));
        }

    }

}
