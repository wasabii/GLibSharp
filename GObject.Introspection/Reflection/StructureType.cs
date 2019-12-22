using System.Runtime.InteropServices;

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
        internal StructureType(IntrospectionContext context) :
            base(context)
        {

        }

        /// <summary>
        /// Gets the layout kind of the type.
        /// </summary>
        public virtual LayoutKind LayoutKind => LayoutKind.Auto;

    }

}
