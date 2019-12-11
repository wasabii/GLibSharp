using System;

namespace GObject.Introspection
{

    public class FunctionAttribute : Attribute
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        public FunctionAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public string CIdentifier { get; set; }

    }

}
