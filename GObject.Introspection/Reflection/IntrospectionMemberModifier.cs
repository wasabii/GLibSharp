using System;

namespace GObject.Introspection.Reflection
{

    [Flags]
    public enum IntrospectionMemberModifier
    {

        Default = 0,

        /// <summary>
        /// The member is a static member.
        /// </summary>
        Static = 1,

    }

}
