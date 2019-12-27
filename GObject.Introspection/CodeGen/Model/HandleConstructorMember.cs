using System;

using GObject.Introspection.CodeGen.Model.Expressions;

namespace GObject.Introspection.CodeGen.Model
{

    /// <summary>
    /// Provides the implementation for the standard Handle property.
    /// </summary>
    class HandleConstructorMember : ConstructorMember
    {

        readonly HandleFieldMember field;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        public HandleConstructorMember(Context context, Type declaringType, HandleFieldMember field) :
            base(context, declaringType)
        {
            this.field = field ?? throw new ArgumentNullException(nameof(field));
        }

        protected override Invokable GetInvokable()
        {
            var self = new ThisParameter(Context, DeclaringType);
            var ptrType = Context.ResolveManagedSymbol(typeof(IntPtr).FullName);
            var handleParameter = new Parameter(Context, "handle", ptrType);

            return new Invokable(
                new[] { handleParameter },
                new ExpressionStatement(Context,
                    new AssignExpression(Context, self.Expression, field.Name, handleParameter.Expression)));
        }

    }

}
