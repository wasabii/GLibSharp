namespace GObject.Introspection.CodeGen.Model
{

    /// <summary>
    /// Describes a member of a class that is an event.
    /// </summary>
    abstract class EventMember : Member
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        internal EventMember(Context context, Type declaringType) :
            base(context, declaringType)
        {

        }

        /// <summary>
        /// Gets the invokable to add a callback.
        /// </summary>
        /// <returns></returns>
        public abstract Invokable GetAddCallbackInvokable();

        /// <summary>
        /// Gets the invokable to remove a callback.
        /// </summary>
        /// <returns></returns>
        public abstract Invokable GetRemoveCallbackInvokable();

    }

}
