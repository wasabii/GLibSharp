using System;

using GObject.Introspection.Model;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes a member of a type mapped to a native function.
    /// </summary>
    class FunctionElementMember : MethodMember
    {

        readonly Function function;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        /// <param name="function"></param>
        public FunctionElementMember(IntrospectionContext context, IntrospectionType declaringType, Function function) :
            base(context, declaringType)
        {
            this.function = function ?? throw new ArgumentNullException(nameof(function));
        }

        /// <summary>
        /// Gets the name of the member.
        /// </summary>
        public override string Name => function.Name;

        public override IntrospectionInvokable GetInvokable()
        {
            throw new NotImplementedException();
        }
    }

}
