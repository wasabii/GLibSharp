using System;
using System.Linq;
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

        /// <summary>
        /// Gets the field type.
        /// </summary>
        /// <returns></returns>
        protected override TypeSymbol GetFieldType()
        {
            // standard field type
            if (field.Type != null)
                return field.Type.ToSymbol(Context);

            // field is an unnamed callback type
            if (field.Callback != null)
            {
                var cb = DeclaringType.Members
                    .OfType<IntrospectionTypeMember>()
                    .Select(i => i.Type).OfType<FieldElementMemberCallbackType>()
                    .FirstOrDefault(i => i.Field == field);
                if (cb is null)
                    throw new InvalidOperationException("Could not locate generated field delegate type.");

                return new IntrospectionTypeSymbol(cb);
            }

            throw new InvalidOperationException();
        }

    }

}
