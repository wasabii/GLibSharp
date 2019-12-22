using System.Runtime.InteropServices;

namespace GObject.Introspection.Model
{

    /// <summary>
    /// Describes an object type within a namespace.
    /// </summary>
    public abstract class StructureType : Type
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        internal StructureType(Context context) :
            base(context)
        {

        }

        /// <summary>
        /// Gets the layout kind of the type.
        /// </summary>
        public virtual LayoutKind LayoutKind => LayoutKind.Auto;

    }

}
