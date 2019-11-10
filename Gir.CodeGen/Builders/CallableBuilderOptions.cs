namespace Gir.CodeGen.Builders
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
        /// <param name="clrTypeName"></param>
        /// <param name="signatureOnly"></param>
        public CallableBuilderOptions(
            GirTypeName? typeName = null,
            ClrTypeName? clrTypeName = null,
            bool? signatureOnly = null)
        {
            if (typeName != null)
                TypeName = typeName;
            if (clrTypeName != null)
                ClrTypeName = clrTypeName;
            if (signatureOnly != null)
                SignatureOnly = (bool)signatureOnly;
        }

        /// <summary>
        /// The currently processing GIR type.
        /// </summary>
        public GirTypeName? TypeName { get; set; }

        /// <summary>
        /// The currently processing CLR type.
        /// </summary>
        public ClrTypeName? ClrTypeName { get; set; }

        /// <summary>
        /// Determines whether callables should be rendered with only a signature.
        /// </summary>
        public bool SignatureOnly { get; set; } = false;

    }

}
