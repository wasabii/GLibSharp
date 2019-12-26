namespace GObject.Introspection.CodeGen.Model
{

    /// <summary>
    /// Describes a type specification.
    /// </summary>
    class TypeSpec
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="nativeType"></param>
        internal TypeSpec(ITypeSymbol type, NativeTypeSymbol nativeType)
        {
            Type = type;
            NativeType = nativeType;
        }

        /// <summary>
        /// Gets the specified type.
        /// </summary>
        public ITypeSymbol Type { get; }

        /// <summary>
        /// Gets the specified native type.
        /// </summary>
        public NativeTypeSymbol NativeType { get; }

        /// <summary>
        /// Gets whether the implementing managed type is blittable.
        /// </summary>
        public virtual bool IsBlittable => false;

    }

}
