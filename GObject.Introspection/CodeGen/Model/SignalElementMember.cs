using System;

using GObject.Introspection.Library.Model;

namespace GObject.Introspection.CodeGen.Model
{

    class SignalElementMember : EventMember
    {

        readonly SignalElement signal;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        /// <param name="signal"></param>
        public SignalElementMember(Context context, Type declaringType, SignalElement signal) :
            base(context, declaringType)
        {
            this.signal = signal ?? throw new ArgumentNullException(nameof(signal));
        }

        /// <summary>
        /// Gets the name of the member.
        /// </summary>
        public override string Name => signal.Name;

        public override Invokable GetAddCallbackInvokable()
        {
            throw new NotImplementedException();
        }

        public override Invokable GetRemoveCallbackInvokable()
        {
            throw new NotImplementedException();
        }

    }

}
