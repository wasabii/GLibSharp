using System;

namespace GObject.Introspection.Model
{

    /// <summary>
    /// Describes an exception that occurred during introspection.
    /// </summary>
    class ModuleException : Exception
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        public ModuleException(string message) :
            base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public ModuleException(string message, Exception innerException) :
            base(message, innerException)
        {

        }

    }

}
