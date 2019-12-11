using System;

using GObject.Introspection.Internal;
using GObject.Introspection.Model;

namespace GObject.Introspection.Reflection
{

    class ConstantMember : IntrospectionMember
    {

        readonly Constant constant;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="constant"></param>
        public ConstantMember(IntrospectionContext context, Constant constant) :
            base(context)
        {
            this.constant = constant ?? throw new ArgumentNullException(nameof(constant));
        }

        /// <summary>
        /// Gets the name of the member.
        /// </summary>
        public override string Name => constant.Name.ToPascalCase();

        /// <summary>
        /// Gets the kind of the member.
        /// </summary>
        public override IntrospectionMemberKind Kind => IntrospectionMemberKind.Field;

        /// <summary>
        /// Gets the value of the member.
        /// </summary>
        public object Value => constant.Value;

    }

}
