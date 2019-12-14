using System;

using GObject.Introspection.Model;

namespace GObject.Introspection.Reflection
{

    class SignalElementMember : EventMember
    {

        readonly Signal signal;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        /// <param name="signal"></param>
        public SignalElementMember(IntrospectionContext context, IntrospectionType declaringType, Signal signal) :
            base(context, declaringType)
        {
            this.signal = signal ?? throw new ArgumentNullException(nameof(signal));
        }

        /// <summary>
        /// Gets the name of the member.
        /// </summary>
        public override string Name => signal.Name;

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
