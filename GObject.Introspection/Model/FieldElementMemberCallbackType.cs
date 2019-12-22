using System;

using GObject.Introspection.Internal;
using GObject.Introspection.Xml;

namespace GObject.Introspection.Model
{

    /// <summary>
    /// Describes the delegate type produced by a field typed as an anonymous callback.
    /// </summary>
    class FieldElementMemberCallbackType : CallbackElementType
    {

        readonly Type parentType;
        readonly FieldElement field;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="callback"></param>
        public FieldElementMemberCallbackType(Context context, Type parentType, FieldElement field) :
            base(context, field.Callback)
        {
            this.parentType = parentType ?? throw new ArgumentNullException(nameof(parentType));
            this.field = field ?? throw new ArgumentNullException(nameof(field));
        }

        /// <summary>
        /// Gets the owning field.
        /// </summary>
        public FieldElement Field => field;

        /// <summary>
        /// Gets the name of the delegate type, derived from the listed name of the callback.
        /// </summary>
        public override string Name => field.Callback.Name.ToPascalCase() + "Func";

        /// <summary>
        /// Gets the qualified name of the delegate type.
        /// </summary>
        public override string QualifiedName => parentType.QualifiedName + "+" + Name;

    }

}
