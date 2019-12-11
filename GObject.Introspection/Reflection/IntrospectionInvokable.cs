using System;
using System.Collections.Generic;
using System.Linq;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes the requirements to dynamically invoke a native introspected object.
    /// </summary>
    public class IntrospectionInvokable
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public IntrospectionInvokable(
            IntrospectionNativeFunction function,
            IEnumerable<IntrospectionArgument> parameters,
            IntrospectionArgument @return,
            IEnumerable<IntrospectionMarshaler> marshalers)
        {
            Function = function ?? throw new ArgumentNullException(nameof(function));
            Parameters = parameters?.ToList() ?? new List<IntrospectionArgument>();
            Return = @return;
            Marshalers = marshalers?.ToList() ?? new List<IntrospectionMarshaler>();
        }

        /// <summary>
        /// Describes the native function to be invoked.
        /// </summary>
        public IntrospectionNativeFunction Function { get; }

        /// <summary>
        /// Gets the argument that describes the return value of the invokable.
        /// </summary>
        public IntrospectionArgument Return { get; }

        /// <summary>
        /// Gets the arguments that describe the parameters of the invokable.
        /// </summary>
        public IReadOnlyList<IntrospectionArgument> Parameters { get; }

        /// <summary>
        /// Describes how marshaling of invokable parameters is conducted.
        /// </summary>
        public IReadOnlyList<IntrospectionMarshaler> Marshalers { get; }

    }

}
