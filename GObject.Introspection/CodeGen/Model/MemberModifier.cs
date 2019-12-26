using System;

namespace GObject.Introspection.CodeGen.Model
{

    [Flags]
    enum MemberModifier
    {

        /// <summary>
        /// The member has no modifier.
        /// </summary>
        Default = 0,

        /// <summary>
        /// The member is a static member.
        /// </summary>
        Static = 1,

        /// <summary>
        /// The member is abstract.
        /// </summary>
        Abstract = 2,

        /// <summary>
        /// The member is virtual.
        /// </summary>
        Virtual = 4,

        /// <summary>
        /// The member is an override.
        /// </summary>
        Override = 8,

    }

}
