using System;

namespace GObject.Introspection
{

    public class MethodAttribute : Attribute
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        public MethodAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }

    }

}
