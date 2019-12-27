using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using GObject.Introspection.CodeGen.Model.Expressions;
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
            var typeSpec = property.Type.ToSpec(Context);
            if (typeSpec == null)
                throw new InvalidOperationException("Cannot resolve type specification for property.");

            var self = new ThisParameter(Context, DeclaringType);
            var ptrType = Context.ResolveManagedSymbol(typeof(IntPtr).FullName);
            var strType = Context.ResolveManagedSymbol(typeof(string).FullName);

            var body = new ReturnStatement(Context,
                new PInvokeExpression(Context,
                    new NativeFunction(
                        "gobject",
                        "g_object_get_property",
                        new Parameter(Context, "object", ptrType),
                        new Parameter(Context, "property_name", strType),
                        new Parameter(Context, "value", ptrType)),
                    new PropertyOrFieldExpression(Context, self.Expression, "Handle", ptrType),
                    new LiteralExpression(Context, property.Name)));

            return new Invokable(
                ImmutableList<Parameter>.Empty,
                typeSpec.Type,
                body);
        }

        /// <summary>
        /// Gets the invokable that describes the setter.
        /// </summary>
        /// <returns></returns>
        public override Invokable GetSetterInvokable()
        {
            var typeSpec = property.Type.ToSpec(Context);
            if (typeSpec == null)
                throw new InvalidOperationException("Cannot resolve type specification for property.");

            var self = new ThisParameter(Context, DeclaringType);
            var value = new Parameter(Context, "value", typeSpec.Type, ParameterModifier.Input);
            var ptrType = Context.ResolveManagedSymbol(typeof(IntPtr).FullName);
            var strType = Context.ResolveManagedSymbol(typeof(string).FullName);

            // returns the results of the g_get_property call, taking the handle of the current class
            var body = new PInvokeStatement(Context,
                new NativeFunction(
                    "gobject",
                    "g_object_set_property",
                    new Parameter(Context, "object", ptrType),
                    new Parameter(Context, "property_name", strType),
                    new Parameter(Context, "value", ptrType)),
                new PropertyOrFieldExpression(Context, self.Expression, "Handle", ptrType),
                new LiteralExpression(Context, property.Name),
                value.Expression);

            return new Invokable(
                new List<Parameter>() { value },
                body);
        }

    }

}
