using System;

namespace GObject.Introspection.Reflection
{

    public abstract class PropertyMember : IntrospectionMember
    {

        readonly Lazy<TypeSymbol> propertyType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        public PropertyMember(IntrospectionContext context, IntrospectionType declaringType) :
            base(context, declaringType)
        {
            propertyType = new Lazy<TypeSymbol>(GetPropertyType);
        }

        /// <summary>
        /// Gets the kind of the member.
        /// </summary>
        public sealed override IntrospectionMemberKind Kind => IntrospectionMemberKind.Property;

        /// <summary>
        /// Gets the type of the property.
        /// </summary>
        public TypeSymbol PropertyType => propertyType.Value;

        /// <summary>
        /// Gets the property type.
        /// </summary>
        /// <returns></returns>
        protected abstract TypeSymbol GetPropertyType();

        /// <summary>
        /// Gets the invokable for the property getter.
        /// </summary>
        /// <returns></returns>
        public abstract IntrospectionInvokable GetGetterInvokable();

        /// <summary>
        /// Gets the invokable for the property setter.
        /// </summary>
        /// <returns></returns>
        public abstract IntrospectionInvokable GetSetterInvokable();

    }

}
