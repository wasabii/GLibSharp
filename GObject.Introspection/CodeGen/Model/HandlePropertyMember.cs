using System;
using System.Collections.Immutable;

using GObject.Introspection.CodeGen.Model.Expressions;

namespace GObject.Introspection.CodeGen.Model
{

    /// <summary>
    /// Provides the implementation for the standard Handle property.
    /// </summary>
    class HandlePropertyMember : PropertyMember
    {

        readonly HandleFieldMember field;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        public HandlePropertyMember(Context context, Type declaringType, HandleFieldMember field) :
            base(context, declaringType)
        {
            this.field = field ?? throw new ArgumentNullException(nameof(field));
        }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        public override string Name => "Handle";

        /// <summary>
        /// Gets the visibility of the getter of the property.
        /// </summary>
        public override Visibility GetterVisibility => Visibility.Public;

        /// <summary>
        /// Gets the invokable for the getter.
        /// </summary>
        /// <returns></returns>
        public override Invokable GetGetterInvokable()
        {
            var self = new ThisParameter(Context, DeclaringType);
            var ptrType = Context.ResolveManagedSymbol(typeof(IntPtr).FullName);

            return new Invokable(
                ImmutableList<Parameter>.Empty,
                Context.ResolveManagedSymbol(typeof(IntPtr).FullName),
                new ReturnStatement(Context,
                    new PropertyOrFieldExpression(Context, self.Expression, field.Name, ptrType)));
        }

        /// <summary>
        /// Gets the visibility of the setter of the property.
        /// </summary>
        public override Visibility SetterVisibility => Visibility.Internal;

        /// <summary>
        /// Gets the invokable for the setter.
        /// </summary>
        /// <returns></returns>
        public override Invokable GetSetterInvokable()
        {
            var self = new ThisParameter(Context, DeclaringType);
            var ptrType = Context.ResolveManagedSymbol(typeof(IntPtr).FullName);
            var valuePrm = new Parameter(Context, "value", ptrType);

            return new Invokable(
                new[] { valuePrm },
                new ExpressionStatement(Context,
                    new AssignExpression(Context, self.Expression, field.Name, valuePrm.Expression)));
        }

    }

}
