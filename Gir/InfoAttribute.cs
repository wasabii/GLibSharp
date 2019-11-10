using System;

using Gir.Model;

namespace Gir
{

    public class InfoAttribute : Attribute
    {

        public bool? Introspectable { get; set; }

        public Stability? Stability { get; set; }

    }

}
