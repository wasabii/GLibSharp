using System;
using System.Collections.Generic;
using System.Linq;

namespace GObject.Introspection.Model
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
            NativeFunction function,
            IEnumerable<Argument> parameters,
            Argument @return,
            IEnumerable<IntrospectionMarshaler> marshalers,
            bool isVarArg = false)
        {
            Function = function ?? throw new ArgumentNullException(nameof(function));
            Parameters = parameters?.ToList() ?? new List<Argument>();
            Return = @return;
            Marshalers = marshalers?.ToList() ?? new List<IntrospectionMarshaler>();
            IsVarArg = isVarArg;
        }

        /// <summary>
        /// Describes the native function to be invoked.
        /// </summary>
        public NativeFunction Function { get; }

        /// <summary>
        /// Gets the argument that describes the return value of the invokable.
        /// </summary>
        public Argument Return { get; }

        /// <summary>
        /// Gets the arguments that describe the parameters of the invokable.
        /// </summary>
        public IReadOnlyList<Argument> Parameters { get; }

        /// <summary>
        /// Gets whether or not the invokable has varadic arguments.
        /// </summary>
        public bool IsVarArg { get; }

        /// <summary>
        /// Describes how marshaling of invokable parameters is conducted.
        /// </summary>
        public IReadOnlyList<IntrospectionMarshaler> Marshalers { get; }

    }

}
