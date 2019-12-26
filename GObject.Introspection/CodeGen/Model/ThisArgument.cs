namespace GObject.Introspection.CodeGen.Model
{

    /// <summary>
    /// Represents the argument that refers to the object instance itself.
    /// </summary>
    class ThisArgument : Argument
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        public ThisArgument(Context context, Type type) :
            base("this", context.ResolveSymbol(type.Name), ArgumentModifier.In)
        {

        }

    }

}
