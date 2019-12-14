using System.Reflection;
using System.Reflection.Emit;

namespace GObject.Introspection.Reflection
{

    public abstract class InterfaceType : IntrospectionType
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public InterfaceType(IntrospectionContext context) :
            base(context)
        {

        }

        public sealed override IntrospectionTypeKind Kind => IntrospectionTypeKind.Interface;

        protected sealed override TypeSymbol GetBaseType()
        {
            return null;
        }

    }

}
