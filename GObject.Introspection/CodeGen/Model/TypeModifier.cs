using System;

namespace GObject.Introspection.CodeGen.Model
{

    [Flags]
    enum Modifier
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
