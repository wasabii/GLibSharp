using System;

using GObject.Introspection.Internal;
using GObject.Introspection.Library.Model;

namespace GObject.Introspection.CodeGen.Model
{

    class PropertyElementMember : PropertyMember
    {

        readonly PropertyElement property;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        /// <param name="property"></param>
        public PropertyElementMember(Context context, Type declaringType, PropertyElement property) :
            base(context, declaringType)
        {
            this.property = property ?? throw new ArgumentNullException(nameof(property));
        }

        /// <summary>
        /// Gets the name of the member.
        /// </summary>
        public override string Name => property.Name.ToPascalCase();

        /// <summary>
        /// Gets the invokable that describes the getter.
        /// </summary>
        /// <returns></returns>
        public override Invokable GetGetterInvokable()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the invokable that describes the setter.
        /// </summary>
        /// <returns></returns>
        public override Invokable GetSetterInvokable()
        {
            throw new NotImplementedException();
        }

    }

}
