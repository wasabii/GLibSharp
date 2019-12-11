using System;

namespace GObject.Introspection
{

    public class VirtualMethodAttribute : Attribute
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        public VirtualMethodAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }

    }

}
