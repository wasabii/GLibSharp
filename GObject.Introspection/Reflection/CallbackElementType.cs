using System;

using GObject.Introspection.Model;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes a delegate generated from a introspected callback.
    /// </summary>
    class CallbackElementType : DelegateType
    {

        readonly Callback callback;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="callback"></param>
        public CallbackElementType(IntrospectionContext context, Callback callback) :
            base(context)
        {
            this.callback = callback ?? throw new ArgumentNullException(nameof(callback));
        }

        /// <summary>
        /// Gets the original introspected name of the type.
        /// </summary>
        public override string IntrospectionName => callback.Name;

        public override string Name => callback.Name;

        public override IntrospectionInvokable GetInvokable()
        {
            throw new NotImplementedException();
        }

    }

}
