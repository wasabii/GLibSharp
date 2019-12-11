using System;

using GObject.Introspection.Model;

namespace GObject.Introspection.Reflection
{

    class SignalMember : EventMember
    {

        readonly Signal signal;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="signal"></param>
        public SignalMember(IntrospectionContext context, Signal signal) :
            base(context)
        {
            this.signal = signal ?? throw new ArgumentNullException(nameof(signal));
        }

        /// <summary>
        /// Gets the name of the member.
        /// </summary>
        public override string Name => signal.Name;

        /// <summary>
        /// Gets the kind of the member.
        /// </summary>
        public override IntrospectionMemberKind Kind => IntrospectionMemberKind.Event;

        public override IntrospectionInvokable GetAddCallbackInvokable()
        {
            throw new NotImplementedException();
        }

        public override IntrospectionInvokable GetRemoveCallbackInvokable()
        {
            throw new NotImplementedException();
        }

    }

}
