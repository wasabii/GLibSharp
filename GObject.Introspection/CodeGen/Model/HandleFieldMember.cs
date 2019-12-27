using System;

namespace GObject.Introspection.CodeGen.Model
{

    class HandleFieldMember : FieldMember
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        public HandleFieldMember(Context context, Type declaringType) :
            base(context, declaringType)
        {

        }

        public override string Name => "handle";

        protected override ITypeSymbol GetFieldType()
        {
            return Context.ResolveManagedSymbol(typeof(IntPtr).FullName);
        }

    }

}
