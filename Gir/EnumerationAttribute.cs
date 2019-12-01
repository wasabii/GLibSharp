using System;

namespace Gir
{

    public class EnumerationAttribute : Attribute
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        public EnumerationAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public string CType { get; set; }

        public string GLibGetType { get; set; }

        public string GLibTypeName { get; set; }

    }

}
