using System;

namespace GObject.Introspection.Reflection
{

    public abstract class FieldMember : IntrospectionMember
    {

        readonly int? offset;
        readonly Lazy<TypeSpec> fieldType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        internal FieldMember(IntrospectionContext context, IntrospectionType declaringType) :
            base(context, declaringType)
        {
            fieldType = new Lazy<TypeSpec>(GetFieldType);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        /// <param name="offset"></param>
        internal FieldMember(IntrospectionContext context, IntrospectionType declaringType, int offset) :
            this(context, declaringType)
        {
            this.offset = offset;
        }

        /// <summary>
        /// Gets the offset of the field.
        /// </summary>
        public virtual int? Offset => offset;

        /// <summary>
        /// Gets the type of the field.
        /// </summary>
        public TypeSpec FieldType => fieldType.Value;

        /// <summary>
        /// Gets the field type.
        /// </summary>
        /// <returns></returns>
        protected abstract TypeSpec GetFieldType();

        /// <summary>
        /// Gets the default value to be assigned to the field.
        /// </summary>
        public virtual object DefaultValue => null;

        /// <summary>
        /// Returns a string representation of this member.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Field {Name}";
        }

    }

}
