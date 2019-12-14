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
        /// <param name="message"></param>
        public SyntaxBuilderException(string message) :
            base(message)
        {

        }

    }

}
