using System;

using Gir.Model;

namespace Gir
{

    /// <summary>
    /// Describes basic GObject information about an object.
    /// </summary>
    public class InfoAttribute : Attribute
    {

        public bool? Introspectable { get; set; }

        public Stability? Stability { get; set; }

    }

}
