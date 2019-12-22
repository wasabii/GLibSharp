using System;
using System.Collections.Generic;

using GObject.Introspection.Internal;
using GObject.Introspection.Xml;

namespace GObject.Introspection.Model
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
        public override IntrospectionInvokable GetInvokable()
        {
            var isVarArg = false;
            var args = new List<Argument>();
            var nativeArgs = new List<NativeArgument>();
            var marshalers = new List<IntrospectionMarshaler>();

            foreach (var parameter in function.Parameters)
            {
                if (parameter is ParameterElement pa)
                {
                    // parameter is a varargs parameter
                    // ignore parameter, but mark method
                    if (pa.VarArgs)
                    {
                        isVarArg = true;
                        continue;
                    }

                    // native type information
                    // TODO derive from required native parameter information
                    var mnt = Context.ResolveManagedSymbol(typeof(IntPtr).FullName);

                    // type native argument as IntPtr for now
                    var na = new NativeArgument(parameter.Name, mnt);
                    nativeArgs.Add(na);

                    // managed argument is resolved parameter type
                    var ma = new Argument(parameter.Name, pa.Type.ToSpec(Context), ArgumentDirection.In);
                    args.Add(ma);

                    // marshal by identity, for now
                    // TODO should generate instructions to derive native type from managed type
                    var mr = new IdentityMarshaler(ma, na);
                    marshalers.Add(mr);
                }
            }

            var returnType = function.ReturnValue?.Type?.ToSpec(Context);
            var returnArg = (Argument)null;
            var nativeReturnArg = (NativeArgument)null;
            if (returnType != null)
            {
                // native type information
                // TODO derive from required native parameter information
                var mnt = Context.ResolveManagedSymbol(typeof(IntPtr).FullName);

                returnArg = new Argument("", returnType, ArgumentDirection.Out);
                nativeReturnArg = new NativeArgument("", mnt);
                marshalers.Add(new IdentityMarshaler(returnArg, nativeReturnArg));
            }

            return new IntrospectionInvokable(
                new NativeFunction("", "", nativeArgs, nativeReturnArg),
                args,
                returnArg,
                marshalers,
                isVarArg);
        }

    }

}
