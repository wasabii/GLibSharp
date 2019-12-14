using System;

namespace GObject.Introspection.Reflection
{

    public abstract class FieldMember : IntrospectionMember
    {

        readonly int? offset;
        readonly Lazy<TypeSymbol> fieldType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        public FieldMember(IntrospectionContext context, IntrospectionType declaringType) :
            base(context, declaringType)
        {
            fieldType = new Lazy<TypeSymbol>(GetFieldType);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        /// <param name="offset"></param>
        public FieldMember(IntrospectionContext context, IntrospectionType declaringType, int offset) :
            this(context, declaringType)
        {
            this.offset = offset;
        }

        /// <summary>
        /// Gets the kind of the member.
        /// </summary>
        public sealed override IntrospectionMemberKind Kind => IntrospectionMemberKind.Field;

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
        protected abstract TypeSymbol GetFieldType();

        /// <summary>
        /// Gets the default value to be assigned to the field.
        /// </summary>
        public virtual object DefaultValue => null;

    }

}
