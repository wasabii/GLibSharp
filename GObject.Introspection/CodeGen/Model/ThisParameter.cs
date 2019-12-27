namespace GObject.Introspection.CodeGen.Model
{

    /// <summary>
    /// Represents the argument that refers to the object instance itself. This is implicitly available for instance
    /// methods.
    /// </summary>
    class ThisParameter : Parameter
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        public ThisParameter(Context context, Type type) :
            base(context, "this", context.ResolveSymbol(type.IntrospectionName), ParameterModifier.Input)
        {

        }

    }

}
