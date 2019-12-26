using System;
using System.Linq;

using GObject.Introspection.Internal;
using GObject.Introspection.Library.Model;

namespace GObject.Introspection.CodeGen.Model
{

    class FieldElementMember : FieldMember
    {

        readonly FieldElement field;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        /// <param name="field"></param>
        public FieldElementMember(Context context, Type declaringType, FieldElement field) :
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
        public FieldElementMember(Context context, Type declaringType, FieldElement field, int offset) :
            base(context, declaringType, offset)
        {
            this.field = field ?? throw new ArgumentNullException(nameof(field));
        }

        public override string Name => field.Name.ToPascalCase();

        /// <summary>
        /// Gets the field type.
        /// </summary>
        /// <returns></returns>
        protected override ITypeSymbol GetFieldType()
        {
            // standard field type
            if (field.Type != null)
                return field.Type.ToSpec(Context).Type;

            // field is an unnamed callback type
            if (field.Callback != null)
            {
                var cb = DeclaringType.Members
                    .OfType<TypeMember>()
                    .Select(i => i.Type).OfType<FieldElementMemberCallbackType>()
                    .FirstOrDefault(i => i.Field == field);
                if (cb is null)
                    throw new InvalidOperationException("Could not locate generated field delegate type.");

                // define inline delegate type
                var typeDef = new TypeDef(DeclaringType.QualifiedName + "+" + cb.Name, cb.IntrospectionName, cb.NativeName, () => cb);
                return new ModuleTypeSymbol(typeDef);
            }

            throw new InvalidOperationException();
        }

    }

}
