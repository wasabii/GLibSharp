using System;

using GObject.Introspection.Internal;
using GObject.Introspection.Model;

namespace GObject.Introspection.Reflection
{

    class FieldElementMember : FieldMember
    {

        readonly Field field;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        /// <param name="field"></param>
        public FieldElementMember(IntrospectionContext context, IntrospectionType declaringType, Field field) :
            base(context, declaringType)
        {
            this.field = field ?? throw new ArgumentNullException(nameof(field));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        /// <param name="field"></param>
        /// <param name="offset"></param>
        public FieldElementMember(IntrospectionContext context, IntrospectionType declaringType, Field field, int offset) :
            base(context, declaringType, offset)
        {
            this.field = field ?? throw new ArgumentNullException(nameof(field));
        }

        public override string Name => field.Name.ToPascalCase();

        protected override TypeSymbol GetFieldType() => field.Type?.ToSymbol(Context);

    }

}
