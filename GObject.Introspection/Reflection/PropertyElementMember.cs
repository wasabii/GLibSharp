using System;

using GObject.Introspection.Internal;
using GObject.Introspection.Model;

namespace GObject.Introspection.Reflection
{

    class PropertyElementMember : PropertyMember
    {

        readonly Property property;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        /// <param name="property"></param>
        public PropertyElementMember(IntrospectionContext context, IntrospectionType declaringType, Property property) :
            base(context, declaringType)
        {
            this.property = property ?? throw new ArgumentNullException(nameof(property));
        }

        /// <summary>
        /// Gets the name of the member.
        /// </summary>
        public override string Name => property.Name.ToPascalCase();

        /// <summary>
        /// Gets the property type.
        /// </summary>
        /// <returns></returns>
        protected override TypeSpec GetPropertyType() => property.Type?.ToSpec(Context);

        public override IntrospectionInvokable GetGetterInvokable()
        {
            throw new NotImplementedException();
        }

        public override IntrospectionInvokable GetSetterInvokable()
        {
            throw new NotImplementedException();
        }

    }

}
