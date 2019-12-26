namespace GObject.Introspection.CodeGen.Model
{

    abstract class InterfaceType : Type
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        internal InterfaceType(Context context) :
            base(context)
        {

        }

        protected sealed override ITypeSymbol GetBaseType()
        {
            return null;
        }

    }

}
