using System;

using GObject.Introspection.Model;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes a member of a type mapped to a native function.
    /// </summary>
    class FunctionMember : MethodMember
    {

        readonly Function function;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="function"></param>
        public FunctionMember(IntrospectionContext context, Function function) :
            base(context)
        {
            this.function = function ?? throw new ArgumentNullException(nameof(function));
        }

        /// <summary>
        /// Gets the name of the member.
        /// </summary>
        public override string Name => function.Name;

        /// <summary>
        /// Gets the kind of the member.
        /// </summary>
        public override IntrospectionMemberKind Kind => IntrospectionMemberKind.Method;

        public override IntrospectionInvokable GetInvokable()
        {
            throw new NotImplementedException();
        }
    }

}
