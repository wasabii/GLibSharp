using System;
using System.Collections.Generic;
using System.Linq;

using GObject.Introspection.Library.Model;

namespace GObject.Introspection.CodeGen.Model
{

    /// <summary>
    /// Describes a flag type generated from a bitfield.
    /// </summary>
    abstract class FlagElementType : EnumType
    {

        readonly FlagElement flag;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="flag"></param>
        internal FlagElementType(Context context, FlagElement flag) :
            base(context)
        {
            this.flag = flag ?? throw new ArgumentNullException(nameof(flag));
        }

        public override string Name => flag.Name;

        public override string IntrospectionName => flag.Name;

        public override string NativeName => flag.CType;

        protected override ITypeSymbol GetBaseType()
        {
            return Context.ResolveManagedSymbol(typeof(int).FullName);
        }

        /// <summary>
        /// Gets whether or not the enum is a flag.
        /// </summary>
        public abstract bool IsFlag { get; }

        protected override IEnumerable<EnumMember> GetMemberMembers()
        {
            return flag.Members.Select(i => new MemberElementMember(Context, this, i));
        }

        protected override IEnumerable<Attribute> GetAttributes()
        {
            if (IsFlag)
                yield return new FlagsAttribute();
        }

    }

}
