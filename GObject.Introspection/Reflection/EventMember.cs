namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes a member of a class that is an event.
    /// </summary>
    public abstract class EventMember : IntrospectionMember
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public EventMember(IntrospectionContext context, IntrospectionType declaringType) :
            base(context, declaringType)
        {

        }

        /// <summary>
        /// Gets the kind of the member.
        /// </summary>
        public sealed override IntrospectionMemberKind Kind => IntrospectionMemberKind.Event;

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
