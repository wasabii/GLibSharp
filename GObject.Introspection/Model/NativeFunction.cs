using System;
using System.Collections.Generic;
using System.Linq;

namespace GObject.Introspection.Model
{

    /// <summary>
    /// Describes a native function call.
    /// </summary>
    public class NativeFunction
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="libraryName"></param>
        /// <param name="entryPoint"></param>
        /// <param name="return"></param>
        /// <param name="parameters"></param>
        public NativeFunction(
            string libraryName,
            string entryPoint,
            IEnumerable<NativeArgument> parameters,
            NativeArgument @return)
        {
            LibraryName = libraryName ?? throw new ArgumentNullException(nameof(libraryName));
            EntryPoint = entryPoint ?? throw new ArgumentNullException(nameof(entryPoint));
            Return = @return;
            Parameters = parameters?.ToList() ?? throw new ArgumentNullException(nameof(parameters));
        }

        /// <summary>
        /// Gets the name of the native library in which the native function resides.
        /// </summary>
        public string LibraryName { get; }

        /// <summary>
        /// Gets the name of the native function to invoke.
        /// </summary>
        public string EntryPoint { get; }

        /// <summary>
        /// Gets the native argument that describes the return value of the native function.
        /// </summary>
        public NativeArgument Return { get; }

        /// <summary>
        /// Gets the native arguments that describe the native function to be invoked.
        /// </summary>
        public IReadOnlyList<NativeArgument> Parameters { get; }

    }

}
