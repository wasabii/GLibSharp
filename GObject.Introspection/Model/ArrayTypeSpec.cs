namespace GObject.Introspection.Model
{

    /// <summary>
    /// Describes an array type specification.
    /// </summary>
    public class ArrayTypeSpec : TypeSpec
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="nativeType"></param>
        /// <param name="itemType"></param>
        /// <param name="fixedSize"></param>
        public ArrayTypeSpec(TypeSymbol type, NativeTypeSymbol nativeType, TypeSpec itemType, int? fixedSize = null) :
            base(type, nativeType)
        {
            ItemType = itemType;
            FixedSize = fixedSize;
        }

        /// <summary>
        /// Gets the specification for the item type.
        /// </summary>
        public TypeSpec ItemType { get; }
        
        /// <summary>
        /// If the specified array type is fixed size, describes that size.
        /// </summary>
        public int? FixedSize { get; }

    }

}
