using System;
using System.Collections.Generic;

using GObject.Introspection.Internal;
using GObject.Introspection.Library.Model;

namespace GObject.Introspection.CodeGen.Model
{

    /// <summary>
    /// Describes a member of a type mapped to a native function.
    /// </summary>
    class FunctionElementMember : MethodMember
    {

        readonly FunctionElement function;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        /// <param name="function"></param>
        public FunctionElementMember(Context context, Type declaringType, FunctionElement function) :
            base(context, declaringType)
        {
            this.function = function ?? throw new ArgumentNullException(nameof(function));
        }

        /// <summary>
        /// Gets the name of the member.
        /// </summary>
        public override string Name => function.Name.ToPascalCase();

        /// <summary>
        /// Gets the modifiers applied to the method.
        /// </summary>
        public override MemberModifier Modifiers => base.Modifiers | MemberModifier.Static;

        /// <summary>
        /// Gets the invokable that represents the function call.
        /// </summary>
        /// <returns></returns>
        protected override Invokable GetInvokable()
        {
            var p = new List<Parameter>();
            for (var i = 0; i < function.Parameters.Count; i++)
            {
                var parameter = (ParameterElement)function.Parameters[i];

                var typeSpec = parameter.Type.ToSpec(Context);
                if (typeSpec == null)
                    throw new InvalidOperationException("Could not derive type specification from parameter.");
            }

            return null;
        }

    }

}
