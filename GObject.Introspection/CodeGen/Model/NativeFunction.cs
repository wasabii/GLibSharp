using System;
using System.Collections.Generic;

namespace GObject.Introspection.CodeGen.Model
{

    /// <summary>
    /// Describes a native function.
    /// </summary>
    class NativeFunction
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="libraryName"></param>
        /// <param name="entryPoint"></param>
        /// <param name="arguments"></param>
        /// <param name="returnArgument"></param>
        public NativeFunction(string libraryName, string entryPoint, IReadOnlyList<Argument> arguments, Argument returnArgument) :
            this(libraryName, entryPoint, arguments)
        {
            ReturnArgument = returnArgument;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="libraryName"></param>
        /// <param name="entryPoint"></param>
        /// <param name="arguments"></param>
        public NativeFunction(string libraryName, string entryPoint, IReadOnlyList<Argument> arguments)
        {
            LibraryName = libraryName ?? throw new ArgumentNullException(nameof(libraryName));
            EntryPoint = entryPoint ?? throw new ArgumentNullException(nameof(entryPoint));
            Arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
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
        /// Gets the native arguments that describe the native function to be invoked.
        /// </summary>
        public IReadOnlyList<Argument> Arguments { get; }

        /// <summary>
        /// Gets the return value of the native function.
        /// </summary>
        public Argument ReturnArgument { get; }

    }

}
