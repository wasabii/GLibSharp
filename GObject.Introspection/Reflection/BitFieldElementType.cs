using System;
using System.Collections.Generic;
using System.Linq;

using GObject.Introspection.Model;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes a flag type generated from a bitfield.
    /// </summary>
    class BitFieldElementType : EnumType
    {

        readonly BitField bitfield;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="flag"></param>
        public BitFieldElementType(IntrospectionContext context, BitField bitfield) :
            base(context)
        {
            this.bitfield = bitfield ?? throw new ArgumentNullException(nameof(bitfield));
        }

        /// <summary>
        /// Gets the original introspected name of the type.
        /// </summary>
        public override string IntrospectionName => bitfield.Name;

        public override string Name => bitfield.Name;

        protected override IEnumerable<EnumerationMember> GetMemberMembers()
        {
            return bitfield.Members.Select(i => new MemberElementMember(Context, this, i));
        }

    }

}
