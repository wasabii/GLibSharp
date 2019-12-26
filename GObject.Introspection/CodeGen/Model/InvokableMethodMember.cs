using System;

namespace GObject.Introspection.CodeGen.Model
{

    /// <summary>
    /// Describes a method implementation that simply accepts an invokable.
    /// </summary>
    class InvokableMethodMember : MethodMember
    {

        readonly string name;
        readonly Invokable invokable;
        readonly Visibility visibility;
        readonly MemberModifier modifers;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        /// <param name="name"></param>
        /// <param name="invokable"></param>
        public InvokableMethodMember(Context context, Type declaringType, string name, Invokable invokable, Visibility visibility, MemberModifier modifers) :
            base(context, declaringType)
        {
            this.name = name ?? throw new ArgumentNullException(nameof(name));
            this.invokable = invokable ?? throw new ArgumentNullException(nameof(invokable));
            this.visibility = visibility;
            this.modifers = modifers;
        }

        public override string Name => name;

        public override Visibility Visibility => visibility;

        public override MemberModifier Modifiers => modifers;

        protected override Invokable GetInvokable() => invokable;

    }

}
