using System;

using GObject.Introspection.Internal;
using GObject.Introspection.Model;

namespace GObject.Introspection.Reflection
{

    class MethodMethodMember : MethodMember
    {

        readonly Method method;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="method"></param>
        public MethodMethodMember(IntrospectionContext context, Method method) :
            base(context)
        {
            this.method = method ?? throw new ArgumentNullException(nameof(method));
        }

        public override string Name => method.Name.ToPascalCase();

        public override IntrospectionInvokable GetInvokable()
        {
            throw new NotImplementedException();
        }
    }

}
