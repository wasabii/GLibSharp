using System;

using GObject.Introspection.Model;

namespace GObject.Introspection.Reflection
{

    class ConstructorMember : MethodMember
    {

        readonly Constructor constructor;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="constructor"></param>
        public ConstructorMember(IntrospectionContext context, Constructor constructor) :
            base(context)
        {
            this.constructor = constructor ?? throw new ArgumentNullException(nameof(constructor));
        }

        /// <summary>
        /// Gets the name of the member.
        /// </summary>
        public override string Name => constructor.Name;

        /// <summary>
        /// Gets the kind of the member.
        /// </summary>
        public override IntrospectionMemberKind Kind => IntrospectionMemberKind.Constructor;

        /// <summary>
        /// Gets the return type of the method.
        /// </summary>
        /// <returns></returns>
        protected override TypeSymbol GetReturnType()
        {
            return null;
        }

        public override IntrospectionInvokable GetInvokable()
        {
            throw new NotImplementedException();
        }

    }

}
