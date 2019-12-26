using System;
using System.Collections.Generic;

namespace GObject.Introspection.CodeGen.Model
{

    /// <summary>
    /// Describes the underlying invocation requirements for a method.
    /// </summary>
    class Invokable
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="returnArgument"></param>
        /// <param name="statements"></param>
        public Invokable(IReadOnlyList<Argument> arguments, Argument returnArgument, IReadOnlyList<Statement> statements) :
            this(arguments, statements)
        {
            ReturnArgument = returnArgument;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="statements"></param>
        public Invokable(IReadOnlyList<Argument> arguments, IReadOnlyList<Statement> statements)
        {
            Arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
            Statements = statements ?? throw new ArgumentNullException(nameof(statements));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="returnArgument"></param>
        /// <param name="statements"></param>
        public Invokable(IReadOnlyList<Argument> arguments, Argument returnArgument, params Statement[] statements) :
            this(arguments, statements)
        {
            ReturnArgument = returnArgument;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="statements"></param>
        public Invokable(IReadOnlyList<Argument> arguments, params Statement[] statements)
        {
            Arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
            Statements = statements ?? throw new ArgumentNullException(nameof(statements));
        }

        /// <summary>
        /// Describes the arguments of the invokable.
        /// </summary>
        public IReadOnlyList<Argument> Arguments { get; }

        /// <summary>
        /// Describes the return value of the invokable.
        /// </summary>
        public Argument ReturnArgument { get; }

        /// <summary>
        /// Gets the statements that make up the body of the invokable.
        /// </summary>
        public IReadOnlyList<Statement> Statements { get; }

    }

}
