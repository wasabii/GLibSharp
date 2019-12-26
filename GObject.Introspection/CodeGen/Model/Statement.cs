namespace GObject.Introspection.CodeGen.Model
{

    abstract class Statement : Evaluable
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public Statement(Context context) :
            base(context)
        {

        }

    }

}
