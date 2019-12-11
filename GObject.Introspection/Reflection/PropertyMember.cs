using System;

using GObject.Introspection.Internal;
using GObject.Introspection.Model;

namespace GObject.Introspection.Reflection
{

    public class PropertyMember : IntrospectionMember
    {

        readonly Property property;
        readonly Lazy<TypeSymbol> propertyType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="property"></param>
        public PropertyMember(IntrospectionContext context, Property property) :
            base(context)
        {
            this.property = property ?? throw new ArgumentNullException(nameof(property));

            propertyType = new Lazy<TypeSymbol>(GetPropertyType);
        }

        /// <summary>
        /// Gets the name of the member.
        /// </summary>
        public override string Name => property.Name.ToPascalCase();

        /// <summary>
        /// Gets the kind of the member.
        /// </summary>
        public override IntrospectionMemberKind Kind => IntrospectionMemberKind.Property;

        /// <summary>
        /// Gets the type of the property.
        /// </summary>
        public TypeSymbol PropertyType => propertyType.Value;

        /// <summary>
        /// Gets the property type.
        /// </summary>
        /// <returns></returns>
        TypeSymbol GetPropertyType()
        {
            return property.Type?.ToSymbol(Context);
        }

        /// <summary>
        /// Gets the invokable for the property getter.
        /// </summary>
        /// <returns></returns>
        public IntrospectionInvokable GetGetterInvokable()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the invokable for the property setter.
        /// </summary>
        /// <returns></returns>
        public IntrospectionInvokable GetSetterInvokable()
        {
            throw new NotImplementedException();
        }

    }

}
