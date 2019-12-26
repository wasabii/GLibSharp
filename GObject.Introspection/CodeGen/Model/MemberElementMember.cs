using System;

using GObject.Introspection.Internal;
using GObject.Introspection.Library.Model;

namespace GObject.Introspection.CodeGen.Model
{

    class MemberElementMember : EnumMember
    {

        readonly MemberElement member;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="member"></param>
        public MemberElementMember(Context context, Type declaringType, MemberElement member) :
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
