using System;

using GObject.Introspection.Xml;

namespace GObject.Introspection
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
