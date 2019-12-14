using System;

using GObject.Introspection.Internal;
using GObject.Introspection.Model;

namespace GObject.Introspection.Reflection
{

    class ConstantElementMember : FieldMember
    {

        readonly Constant constant;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="constant"></param>
        public ConstantElementMember(IntrospectionContext context, IntrospectionType declaringType, Constant constant) :
            base(context, declaringType)
        {
            this.constant = constant ?? throw new ArgumentNullException(nameof(constant));
        }

        public override string Name => constant.Name.ToPascalCase();

        public override object DefaultValue => constant.Value;

        protected override TypeSymbol GetFieldType() => constant.Type?.ToSymbol(Context);

    }

}
