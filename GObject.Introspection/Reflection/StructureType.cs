namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes an object type within a namespace.
    /// </summary>
    public abstract class StructureType : IntrospectionType
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public StructureType(IntrospectionContext context) :
            base(context)
        {

        }

        public sealed override IntrospectionTypeKind Kind => IntrospectionTypeKind.Structure;

    }

}
