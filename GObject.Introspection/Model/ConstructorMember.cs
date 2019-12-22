namespace GObject.Introspection.Model
{

    public abstract class ConstructorMember : MethodMember
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        internal ConstructorMember(Context context, Type declaringType) :
            base(context, declaringType)
        {

        }

        /// <summary>
        /// Gets the name of the member.
        /// </summary>
        public override sealed string Name => null;

        /// <summary>
        /// Gets the return type of the method.
        /// </summary>
        /// <returns></returns>
        protected override sealed TypeSpec GetReturnType()
        {
            return null;
        }

        public override string ToString()
        {
            return Name;
        }

    }

}