namespace GObject.Introspection.CodeGen.Model
{

    /// <summary>
    /// Describes the direction of an introspected invokable.
    /// </summary>
    enum ArgumentModifier
    {

        /// <summary>
        /// Argument is passed into native method.
        /// </summary>
        In,

        /// <summary>
        /// Argument represents an output of the native method.
        /// </summary>
        Out,

        /// <summary>
        /// Argument is passed to the native method by reference.
        /// </summary>
        Ref,

    }

}
