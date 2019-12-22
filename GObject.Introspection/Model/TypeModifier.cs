using System;

namespace GObject.Introspection.Model
{

    [Flags]
    public enum TypeModifier
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
