using System;
using System.Collections.Generic;
using System.Linq;

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
        /// <param name="parameters"></param>
        /// <param name="returnType"></param>
        /// <param name="statements"></param>
        public Invokable(IReadOnlyList<Parameter> parameters, ITypeSymbol returnType, IReadOnlyList<Statement> statements) :
            this(parameters, statements)
        {
            ReturnType = returnType;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="statements"></param>
        public Invokable(IReadOnlyList<Parameter> parameters, IReadOnlyList<Statement> statements)
        {
            Parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
            Statements = statements ?? throw new ArgumentNullException(nameof(statements));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="returnType"></param>
        /// <param name="statements"></param>
        public Invokable(IReadOnlyList<Parameter> parameters, ITypeSymbol returnType, params Statement[] statements) :
            this(parameters, returnType, statements.ToList())
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="returnType"></param>
        /// <param name="statements"></param>
        public Invokable(Parameter[] parameters, ITypeSymbol returnType, params Statement[] statements) :
            this(parameters.ToList(), returnType, statements.ToList())
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="statements"></param>
        public Invokable(IReadOnlyList<Parameter> parameters, params Statement[] statements) :
            this(parameters, statements.ToList())
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="statements"></param>
        public Invokable(Parameter[] parameters, params Statement[] statements) :
            this(parameters.ToList(), statements.ToList())
        {

        }

        /// <summary>
        /// Describes the parameters of the invokable.
        /// </summary>
        public IReadOnlyList<Parameter> Parameters { get; }

        /// <summary>
        /// Describes the return value of the invokable.
        /// </summary>
        public ITypeSymbol ReturnType { get; }

        /// <summary>
        /// Gets the statements that make up the body of the invokable.
        /// </summary>
        public IReadOnlyList<Statement> Statements { get; }

    }

}
