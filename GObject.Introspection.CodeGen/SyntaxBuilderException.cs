using System;

namespace GObject.Introspection.CodeGen
{

    /// <summary>
    /// Describes an exception that occurred during syntax generation.
    /// </summary>
    public class SyntaxBuilderException : Exception
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="message"></param>
        public SyntaxBuilderException(IContext context, string message) :
            base(message)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Gets the context at which the exception was created.
        /// </summary>
        public IContext Context { get; }

    }

}
