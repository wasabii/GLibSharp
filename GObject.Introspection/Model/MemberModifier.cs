using System;

namespace GObject.Introspection.Model
{

    [Flags]
    public enum MemberModifier
    {

        Default = 0,

        /// <summary>
        /// The member is a static member.
        /// </summary>
        Static = 1,

    }

}
