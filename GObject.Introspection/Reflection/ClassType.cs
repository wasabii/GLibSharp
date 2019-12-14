using System.Reflection;
using System.Reflection.Emit;

namespace GObject.Introspection.Reflection
{

    public abstract class ClassType : IntrospectionType
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public ClassType(IntrospectionContext context) :
            base(context)
        {

        }

        public sealed override IntrospectionTypeKind Kind => IntrospectionTypeKind.Class;

    }

}
