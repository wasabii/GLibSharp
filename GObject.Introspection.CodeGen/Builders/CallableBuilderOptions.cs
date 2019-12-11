namespace GObject.Introspection.CodeGen.Builders
{

    /// <summary>
    /// Options made available to the method scope.
    /// </summary>
    public class CallableBuilderOptions
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public CallableBuilderOptions()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="typeInfo"></param>
        /// <param name="signatureOnly"></param>
        public CallableBuilderOptions(TypeInfo typeInfo = null, bool? signatureOnly = null)
        {
            if (typeInfo != null)
                TypeInfo = typeInfo;
            if (signatureOnly != null)
                SignatureOnly = (bool)signatureOnly;
        }

        /// <summary>
        /// The currently processing CLR type.
        /// </summary>
        public TypeInfo TypeInfo { get; set; }

        /// <summary>
        /// Determines whether callables should be rendered with only a signature.
        /// </summary>
        public bool SignatureOnly { get; set; } = false;

    }

}
