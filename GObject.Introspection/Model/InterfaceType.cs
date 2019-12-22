namespace GObject.Introspection.Model
{

    public abstract class InterfaceType : Type
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        internal InterfaceType(Context context) :
            base(context)
        {

        }

        protected sealed override TypeSymbol GetBaseType()
        {
            return null;
        }

    }

}
