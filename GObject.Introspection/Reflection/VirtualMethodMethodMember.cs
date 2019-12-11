using System;

using GObject.Introspection.Internal;
using GObject.Introspection.Model;

namespace GObject.Introspection.Reflection
{

    class VirtualMethodMethodMember : MethodMember
    {

        readonly VirtualMethod method;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="method"></param>
        public VirtualMethodMethodMember(IntrospectionContext context, VirtualMethod method) :
            base(context)
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
