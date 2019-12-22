using System;

namespace GObject.Introspection.Xml
{

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class GirException : Exception
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public GirException() : base()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        public GirException(string message) : base(message)
        {

        }

    }

}