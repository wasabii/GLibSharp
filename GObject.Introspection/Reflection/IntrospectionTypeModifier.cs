using System;

namespace GObject.Introspection.Reflection
{

    [Flags]
    public enum IntrospectionTypeModifier
    {

        Default = 0,

        /// <summary>
        /// The type is a static type.
        /// </summary>
        Static = 1,

        /// <summary>
        /// The type is an abstract type.
        /// </summary>
        Abstract = 2,

    }

}
