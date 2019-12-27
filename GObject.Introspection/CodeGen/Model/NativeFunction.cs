using System;
using System.Collections.Generic;
using System.Linq;

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
        /// <param name="parameters"></param>
        /// <param name="returnType"></param>
        public NativeFunction(string libraryName, string entryPoint, ITypeSymbol returnType, IReadOnlyList<Parameter> parameters) :
            this(libraryName, entryPoint, parameters)
        {
            ReturnType = returnType;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="libraryName"></param>
        /// <param name="entryPoint"></param>
        /// <param name="parameters"></param>
        /// <param name="returnType"></param>
        public NativeFunction(string libraryName, string entryPoint, ITypeSymbol returnType, params Parameter[] parameters) :
            this(libraryName, entryPoint, returnType, parameters.ToList())
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="libraryName"></param>
        /// <param name="entryPoint"></param>
        /// <param name="parameters"></param>
        public NativeFunction(string libraryName, string entryPoint, IReadOnlyList<Parameter> parameters)
        {
            LibraryName = libraryName ?? throw new ArgumentNullException(nameof(libraryName));
            EntryPoint = entryPoint ?? throw new ArgumentNullException(nameof(entryPoint));
            Parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="libraryName"></param>
        /// <param name="entryPoint"></param>
        /// <param name="parameters"></param>
        public NativeFunction(string libraryName, string entryPoint, params Parameter[] parameters) :
            this(libraryName, entryPoint, parameters.ToList())
        {

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
        /// Gets the return value of the native function.
        /// </summary>
        public ITypeSymbol ReturnType { get; }

        /// <summary>
        /// Gets the parameters that describe the native function to be invoked.
        /// </summary>
        public IReadOnlyList<Parameter> Parameters { get; }

    }

}
