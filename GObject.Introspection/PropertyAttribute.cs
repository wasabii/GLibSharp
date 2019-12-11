using System;

namespace GObject.Introspection
{

    public class PropertyAttribute : Attribute
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        public PropertyAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }

    }

}
