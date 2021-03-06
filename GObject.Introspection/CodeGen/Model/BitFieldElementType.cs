﻿using System;
using System.Collections.Generic;
using System.Linq;

using GObject.Introspection.Library.Model;

namespace GObject.Introspection.CodeGen.Model
{

    /// <summary>
    /// Describes a flag type generated from a bitfield.
    /// </summary>
    class BitFieldElementType : FlagElementType
    {

        readonly BitFieldElement bitfield;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bitfield"></param>
        public BitFieldElementType(Context context, BitFieldElement bitfield) :
            base(context, bitfield)
        {
            this.bitfield = bitfield ?? throw new ArgumentNullException(nameof(bitfield));
        }

        public override bool IsFlag => true;

        protected override IEnumerable<EnumMember> GetMemberMembers()
        {
            return bitfield.Members.Select(i => new MemberElementMember(Context, this, i));
        }

    }

}
