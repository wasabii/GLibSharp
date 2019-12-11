using System;

using GObject.Introspection.Internal;
using GObject.Introspection.Model;

namespace GObject.Introspection.Reflection
{

    class EnumerationMember : IntrospectionMember
    {

        readonly Member member;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="member"></param>
        public EnumerationMember(IntrospectionContext context, Member member) :
            base(context)
        {
            this.member = member ?? throw new ArgumentNullException(nameof(member));
        }

        /// <summary>
        /// Gets the name of the member.
        /// </summary>
        public override string Name => member.Name.ToPascalCase();

        /// <summary>
        /// Gets the kind of the member.
        /// </summary>
        public override IntrospectionMemberKind Kind => IntrospectionMemberKind.Member;

    }

}
