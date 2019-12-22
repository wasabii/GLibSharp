namespace GObject.Introspection.Reflection
{

    public abstract class InterfaceType : IntrospectionType
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        internal InterfaceType(IntrospectionContext context) :
            base(context)
        {

        }

        protected sealed override TypeSymbol GetBaseType()
        {
            return null;
        }

    }

}
