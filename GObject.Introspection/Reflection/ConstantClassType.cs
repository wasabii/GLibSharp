using System;
using System.Collections.Generic;
using System.Linq;

using GObject.Introspection.Model;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes a constant type.
    /// </summary>
    class ConstantClassType : ClassType
    {

        readonly List<Constant> constants;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="constants"></param>
        public ConstantClassType(IntrospectionContext context, List<Constant> constants) :
            base(context)
        {
            this.constants = constants ?? throw new ArgumentNullException(nameof(constants));
        }

        /// <summary>
        /// Gets the original introspected name of the type.
        /// </summary>
        public override string IntrospectionName => null;

        public override string Name => "Constants";

        protected override IEnumerable<IntrospectionMember> GetMembers()
        {
            return base.GetMembers()
                .Concat(GetConstantMembers());
        }

        protected virtual IEnumerable<FieldMember> GetConstantMembers()
        {
            return constants.Select(i => new ConstantElementMember(Context, this, i));
        }

    }

}
