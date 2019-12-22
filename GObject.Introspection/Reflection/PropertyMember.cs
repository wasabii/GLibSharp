using System;

namespace GObject.Introspection.Reflection
{

    public abstract class PropertyMember : IntrospectionMember
    {

        readonly Lazy<TypeSpec> propertyType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        internal PropertyMember(IntrospectionContext context, IntrospectionType declaringType) :
            base(context, declaringType)
        {
            propertyType = new Lazy<TypeSpec>(GetPropertyType);
        }

        /// <summary>
        /// Gets the type of the property.
        /// </summary>
        public TypeSpec PropertyType => propertyType.Value;

        /// <summary>
        /// Gets the property type.
        /// </summary>
        /// <returns></returns>
        protected abstract TypeSpec GetPropertyType();

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
