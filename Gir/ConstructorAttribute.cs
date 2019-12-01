using System;

namespace Gir
{

    public class ConstructorAttribute : Attribute
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        public ConstructorAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }

    }

}
