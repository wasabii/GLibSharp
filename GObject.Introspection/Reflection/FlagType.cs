using System;
using System.Collections.Generic;
using System.Linq;

using GObject.Introspection.Model;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes a flag type generated from a bitfield.
    /// </summary>
    abstract class FlagType : IntrospectionType
    {

        readonly Flag flag;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="flag"></param>
        public FlagType(IntrospectionContext context, Flag flag) :
            base(context)
        {
            this.flag = flag ?? throw new ArgumentNullException(nameof(flag));
        }

        /// <summary>
        /// Gets the original introspected name of the type.
        /// </summary>
        public override string IntrospectionName => flag.Name;

        public override string Name => flag.Name;

        public override IntrospectionTypeKind Kind => IntrospectionTypeKind.Enum;

        protected override IEnumerable<IntrospectionMember> GetMembers()
        {
            return base.GetMembers().Concat(GetMemberMembers());
        }

        protected virtual IEnumerable<IntrospectionMember> GetMemberMembers()
        {
            return flag.Members.Select(i => new EnumerationMember(Context, i));
        }

    }

}
