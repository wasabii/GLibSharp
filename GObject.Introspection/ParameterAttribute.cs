using System;

using GObject.Introspection.Model;

namespace GObject.Introspection
{

    public class ParameterAttribute : Attribute
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        public ParameterAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public bool? Nullable { get; set; }

        public bool? CallerAllocates { get; set; }

        public TransferOwnership? TransferOwnership { get; set; }

    }

}
