using System;
using System.Collections.Generic;
using System.Linq;

using GObject.Introspection.Model;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes a flag type generated from a bitfield.
    /// </summary>
    class BitFieldElementType : FlagElementType
    {

        readonly BitField bitfield;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bitfield"></param>
        public BitFieldElementType(IntrospectionContext context, BitField bitfield) :
            base(context, bitfield)
        {
            this.bitfield = bitfield ?? throw new ArgumentNullException(nameof(bitfield));
        }

        protected override IEnumerable<EnumMember> GetMemberMembers()
        {
            return bitfield.Members.Select(i => new MemberElementMember(Context, this, i));
        }

    }

}
