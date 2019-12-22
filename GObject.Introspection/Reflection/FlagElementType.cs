using System;
using System.Collections.Generic;
using System.Linq;

using GObject.Introspection.Model;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes a flag type generated from a bitfield.
    /// </summary>
    public abstract class FlagElementType : EnumType
    {

        readonly Flag flag;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="flag"></param>
        internal FlagElementType(IntrospectionContext context, Flag flag) :
            base(context)
        {
            this.flag = flag ?? throw new ArgumentNullException(nameof(flag));
        }

        public override string Name => flag.Name;

        public override string IntrospectionName => flag.Name;

        public override string NativeName => flag.CType;

        public override TypeSymbol BaseType => Context.ResolveManagedSymbol(typeof(int).FullName);

        protected override IEnumerable<EnumMember> GetMemberMembers()
        {
            return flag.Members.Select(i => new MemberElementMember(Context, this, i));
        }

    }

}
