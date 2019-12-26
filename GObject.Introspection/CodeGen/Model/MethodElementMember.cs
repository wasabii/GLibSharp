using System;

using GObject.Introspection.Internal;
using GObject.Introspection.Library.Model;

namespace GObject.Introspection.CodeGen.Model
{

    class MethodElementMember : MethodMember
    {

        readonly MethodElement method;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        /// <param name="method"></param>
        public MethodElementMember(Context context, Type declaringType, MethodElement method) :
            base(context, declaringType)
        {
            this.method = method ?? throw new ArgumentNullException(nameof(method));
        }

        public override string Name => method.Name.ToPascalCase();

        protected override Invokable GetInvokable()
        {
            return null;
        }

    }

}
