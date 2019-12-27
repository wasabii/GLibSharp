namespace GObject.Introspection.CodeGen.Model
{

    /// <summary>
    /// Describes the direction of an introspected invokable.
    /// </summary>
    enum ParameterModifier
    {

        /// <summary>
        /// Parameter is passed into method.
        /// </summary>
        Input,

        /// <summary>
        /// Parameter represents an output of the method.
        /// </summary>
        Output,

        /// <summary>
        /// Parameter is passed to the method by reference.
        /// </summary>
        Reference,

        /// <summary>
        /// Parameter is the return value of a method.
        /// </summary>
        Return,

    }

}
