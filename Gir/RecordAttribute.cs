using System;

namespace Gir
{

    /// <summary>
    /// Describes a GObject record type.
    /// </summary>
    public class RecordAttribute : Attribute
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        public RecordAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }

    }

}
