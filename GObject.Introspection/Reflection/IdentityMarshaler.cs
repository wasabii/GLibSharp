using System;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes marshaling a parameter to a native method merely by passing the same value.
    /// </summary>
    public class IdentityMarshaler : IntrospectionMarshaler
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="nativeArgument"></param>
        public IdentityMarshaler(IntrospectionArgument argument, IntrospectionNativeArgument nativeArgument)
        {
            Argument = argument ?? throw new ArgumentNullException(nameof(argument));
            NativeArgument = nativeArgument ?? throw new ArgumentNullException(nameof(nativeArgument));
        }

        /// <summary>
        /// The managed argument to be marshaled.
        /// </summary>
        public IntrospectionArgument Argument { get; }

        /// <summary>
        /// The native argument to be marshaled to.
        /// </summary>
        public IntrospectionNativeArgument NativeArgument { get; }

    }

}
