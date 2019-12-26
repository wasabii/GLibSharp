using System;

namespace GObject.Introspection.CodeGen.Model
{

    /// <summary>
    /// Describes something that can be evaluated.
    /// </summary>
    abstract class Evaluable
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        public Evaluable(Context context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Gets the context of the expression.
        /// </summary>
        public Context Context { get; }

    }

}
