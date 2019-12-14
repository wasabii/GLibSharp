using System;

using GObject.Introspection.Internal;
using GObject.Introspection.Model;

namespace GObject.Introspection.Reflection
{

    class MemberElementMember : EnumerationMember
    {

        readonly Member member;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="member"></param>
        public MemberElementMember(IntrospectionContext context, IntrospectionType declaringType, Member member) :
            base(context, declaringType)
        {
            this.member = member ?? throw new ArgumentNullException(nameof(member));
        }

        /// <summary>
        /// Gets the name of the member.
        /// </summary>
        public override string Name => member.Name.ToPascalCase();

        /// <summary>
        /// Gets the value of the enumeration member.
        /// </summary>
        public override int Value => (int)long.Parse(member.Value);

    }

}
