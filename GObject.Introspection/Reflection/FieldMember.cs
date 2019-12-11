using System;

using GObject.Introspection.Internal;
using GObject.Introspection.Model;

namespace GObject.Introspection.Reflection
{

    public class FieldMember : IntrospectionMember
    {

        readonly Field field;
        readonly int? offset;
        readonly Lazy<TypeSymbol> fieldType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="field"></param>
        public FieldMember(IntrospectionContext context, Field field) :
            base(context)
        {
            this.field = field ?? throw new ArgumentNullException(nameof(field));

            fieldType = new Lazy<TypeSymbol>(GetFieldType);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="field"></param>
        /// <param name="offset"></param>
        public FieldMember(IntrospectionContext context, Field field, int offset) :
            this(context, field)
        {
            this.offset = offset;
        }

        /// <summary>
        /// Gets the name of the member.
        /// </summary>
        public override string Name => field.Name.ToPascalCase();

        /// <summary>
        /// Gets the kind of the member.
        /// </summary>
        public override IntrospectionMemberKind Kind => IntrospectionMemberKind.Field;

        /// <summary>
        /// Gets the offset of the field.
        /// </summary>
        public virtual int? Offset => offset;

        /// <summary>
        /// Gets the type of the field.
        /// </summary>
        public TypeSymbol FieldType => fieldType.Value;

        /// <summary>
        /// Gets the field type.
        /// </summary>
        /// <returns></returns>
        TypeSymbol GetFieldType()
        {
            return field.Type?.ToSymbol(Context);
        }

        /// <summary>
        /// Gets the invokable for the field getter.
        /// </summary>
        /// <returns></returns>
        public IntrospectionInvokable GetGetterInvokable()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the invokable for the field setter.
        /// </summary>
        /// <returns></returns>
        public IntrospectionInvokable GetSetterInvokable()
        {
            throw new NotImplementedException();
        }

    }

}
