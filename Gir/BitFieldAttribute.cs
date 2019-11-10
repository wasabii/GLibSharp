using System;

namespace Gir
{

    public class BitFieldAttribute : Attribute
    {

        public string Name { get; set; }

        public string CType { get; set; }

        public string GLibGetType { get; set; }

        public string GLibTypeName { get; set; }

    }

}
