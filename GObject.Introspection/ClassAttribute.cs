using System;

namespace GObject.Introspection
{

    public class ClassAttribute : Attribute
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        public ClassAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }

    }

}
