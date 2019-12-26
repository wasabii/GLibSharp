namespace GObject.Introspection.CodeGen.Model
{

    abstract class ConstructorMember : MethodMember
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

        public override string ToString()
        {
            return Name;
        }

    }

}