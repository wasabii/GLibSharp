using System;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes an exception that occurred during introspection.
    /// </summary>
    class IntrospectionException : Exception
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        public IntrospectionException(string message) :
            base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public IntrospectionException(string message, Exception innerException) :
            base(message, innerException)
        {

        }

    }

}
