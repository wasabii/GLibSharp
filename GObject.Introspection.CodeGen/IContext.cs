using System;
using System.Collections.Generic;

using GObject.Introspection.Model;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace GObject.Introspection.CodeGen
{

    /// <summary>
    /// Provides information about the processing context.
    /// </summary>
    public interface IContext
    {

        /// <summary>
        /// Gets a string that identifies the current context.
        /// </summary>
        string DebugText { get; }

        /// <summary>
        /// Gets the GIR namespace currently being processed.
        /// </summary>
        string CurrentNamespace { get; }

        /// <summary>
        /// Namespace import list. Should be resolved from the end.
        /// </summary>
        IList<string> Imports { get; }

        /// <summary>
        /// Gets the namespace resolver.
        /// </summary>
        IRepositoryProvider Repositories { get; }

        /// <summary>
        /// CLR symbol mapping.
        /// </summary>
        ITypeInfoProvider Types { get; }

        /// <summary>
        /// Provides the current <see cref="SyntaxGenerator"/>.
        /// </summary>
        SyntaxGenerator Syntax { get; }

        /// <summary>
        /// Gets an annotation of the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Annotation<T>();

        /// <summary>
        /// Initiates a build for the given element, possibly with a new context.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        IEnumerable<SyntaxNode> Build(Element element);

        /// <summary>
        /// Initiates a new context with an import added.
        /// </summary>
        /// <param name="ns"></param>
        /// <returns></returns>
        IContext WithImport(string ns);

        /// <summary>
        /// Initiates a new context with some modified values.
        /// </summary>
        /// <param name="defaultNamespace"></param>
        /// <returns></returns>
        IContext WithNamespace(string defaultNamespace);

        /// <summary>
        /// Initiates a new context with an additional annotation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="annotation"></param>
        /// <returns></returns>
        IContext WithAnnotation<T>(T annotation);

        /// <summary>
        /// Creates a new context with debug text.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IContext WithDebugText(string name);

        /// <summary>
        /// Adds an debug message to the context.
        /// </summary>
        /// <param name="message"></param>
        IContext Debug(FormattableString message);

        /// <summary>
        /// Adds an warning message to the context.
        /// </summary>
        /// <param name="message"></param>
        IContext Warning(FormattableString message);

        /// <summary>
        /// Adds an error message to the context.
        /// </summary>
        /// <param name="message"></param>
        IContext Error(FormattableString message);

        /// <summary>
        /// Generates a new exception given the context.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Exception Exception(FormattableString message);

    }

}
