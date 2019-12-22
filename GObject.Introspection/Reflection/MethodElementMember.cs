using System;
using System.Collections.Generic;
using GObject.Introspection.Internal;
using GObject.Introspection.Model;

namespace GObject.Introspection.Reflection
{

    class MethodElementMember : MethodMember
    {

        readonly Method method;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        /// <param name="method"></param>
        public MethodElementMember(IntrospectionContext context, IntrospectionType declaringType, Method method) :
            base(context, declaringType)
        {
            this.method = method ?? throw new ArgumentNullException(nameof(method));
        }

        public override string Name => method.Name.ToPascalCase();

        /// <summary>
        /// Gets the invokable that represents the function call.
        /// </summary>
        /// <returns></returns>
        public override IntrospectionInvokable GetInvokable()
        {
            var isVarArg = false;
            var args = new List<IntrospectionArgument>();
            var nativeArgs = new List<IntrospectionNativeArgument>();
            var marshalers = new List<IntrospectionMarshaler>();

            foreach (var parameter in method.Parameters)
            {
                if (parameter is Parameter pa)
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
                    var na = new IntrospectionNativeArgument(parameter.Name, mnt);
                    nativeArgs.Add(na);

                    // managed argument is resolved parameter type
                    var ma = new IntrospectionArgument(parameter.Name, pa.Type.ToSpec(Context), IntrospectionArgumentDirection.In);
                    args.Add(ma);

                    // marshal by identity, for now
                    // TODO should generate instructions to derive native type from managed type
                    var mr = new IdentityMarshaler(ma, na);
                    marshalers.Add(mr);
                }
            }

            var returnType = method.ReturnValue?.Type?.ToSpec(Context);
            var returnArg = (IntrospectionArgument)null;
            var nativeReturnArg = (IntrospectionNativeArgument)null;
            if (returnType != null)
            {
                // native type information
                // TODO derive from required native parameter information
                var mnt = Context.ResolveManagedSymbol(typeof(IntPtr).FullName);

                returnArg = new IntrospectionArgument("", returnType, IntrospectionArgumentDirection.Out);
                nativeReturnArg = new IntrospectionNativeArgument("", mnt);
                marshalers.Add(new IdentityMarshaler(returnArg, nativeReturnArg));
            }

            return new IntrospectionInvokable(
                new IntrospectionNativeFunction("", "", nativeArgs, nativeReturnArg),
                args,
                returnArg,
                marshalers,
                isVarArg);
        }

    }

}
