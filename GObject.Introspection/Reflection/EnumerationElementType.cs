using System;
using System.Collections.Generic;
using System.Linq;

using GObject.Introspection.Model;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes a flag type generated from an enumeration.
    /// </summary>
    class EnumerationElementType : EnumType
    {

        readonly Enumeration enumeration;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="flag"></param>
        public EnumerationElementType(IntrospectionContext context, Enumeration enumeration) :
            base(context)
        {
            this.enumeration = enumeration ?? throw new ArgumentNullException(nameof(enumeration));
        }

        /// <summary>
        /// Gets the original introspected name of the type.
        /// </summary>
        public override string IntrospectionName => enumeration.Name;

        public override string Name => enumeration.Name;

        protected override IEnumerable<EnumerationMember> GetMemberMembers()
        {
            return enumeration.Members.Select(i => new MemberElementMember(Context, this, i));
        }

    }

}
