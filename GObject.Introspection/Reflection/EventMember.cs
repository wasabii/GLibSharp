namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes a member of a class that is an event.
    /// </summary>
    abstract class EventMember : IntrospectionMember
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public EventMember(IntrospectionContext context) :
            base(context)
        {

        }

        /// <summary>
        /// Gets the invokable to add a callback.
        /// </summary>
        /// <returns></returns>
        public abstract IntrospectionInvokable GetAddCallbackInvokable();

        /// <summary>
        /// Gets the invokable to remove a callback.
        /// </summary>
        /// <returns></returns>
        public abstract IntrospectionInvokable GetRemoveCallbackInvokable();

    }

}
