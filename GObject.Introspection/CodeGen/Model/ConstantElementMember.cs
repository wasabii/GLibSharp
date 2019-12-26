using System;

using GObject.Introspection.Internal;
using GObject.Introspection.Library.Model;

namespace GObject.Introspection.CodeGen.Model
{

    class ConstantElementMember : FieldMember
    {

        readonly ConstantElement constant;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="constant"></param>
        public ConstantElementMember(Context context, Type declaringType, ConstantElement constant) :
            base(context, declaringType)
        {
            this.constant = constant ?? throw new ArgumentNullException(nameof(constant));
        }

        public override string Name => constant.Name.ToPascalCase();

        public override object DefaultValue => constant.Value;

        protected override ITypeSymbol GetFieldType() => constant.Type?.ToSpec(Context).Type;

    }

}
