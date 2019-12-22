using System;

using GObject.Introspection.Internal;
using GObject.Introspection.Xml;

namespace GObject.Introspection.Model
{

    class VirtualMethodElementMember : MethodMember
    {

        readonly VirtualMethodElement method;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        /// <param name="method"></param>
        public VirtualMethodElementMember(Context context, Type declaringType, VirtualMethodElement method) :
            base(context, declaringType)
        {
            this.method = method ?? throw new ArgumentNullException(nameof(method));
        }

        /// <summary>
        /// Gets the name of the method.
        /// </summary>
        public override string Name => method.Name.ToPascalCase();

        public override IntrospectionInvokable GetInvokable()
        {
            throw new NotImplementedException();
        }

    }

}
