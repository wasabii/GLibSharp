using System;
using Gir.Model;

namespace Gir
{

    public class ParameterAttribute : Attribute
    {

        public string Name { get; set; }

        public bool? Nullable { get; set; }

        public bool? CallerAllocates { get; set; }

        public TransferOwnership? TransferOwnership { get; set; }

    }

}
