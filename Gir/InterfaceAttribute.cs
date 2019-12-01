using System;

namespace Gir
{

    public class InterfaceAttribute : Attribute
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        public InterfaceAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }

    }

}
