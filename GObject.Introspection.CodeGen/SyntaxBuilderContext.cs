using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using GObject.Introspection.Model;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace GObject.Introspection.CodeGen
{

    /// <summary>
    /// Provides a context object for this builder.
    /// </summary>
    class SyntaxBuilderContext : IContext
    {

        readonly SyntaxBuilderContext parent;
        readonly string debugText;
        readonly string defaultNamespace;
        readonly ImmutableList<string> imports;
        readonly SyntaxBuilder builder;
        readonly SyntaxGenerator syntax;
        readonly IRepositoryProvider repositories;
        readonly ITypeInfoProvider clrTypeInfo;
        readonly ImmutableList<object> annotations;
        readonly Action<SyntaxBuilderMessage> messageFunc;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="defaultNamespace"></param>
        /// <param name="imports"></param>
        /// <param name="builder"></param>
        /// <param name="syntax"></param>
        /// <param name="repositories"></param>
        /// <param name="clrTypeInfo"></param>
        /// <param name="annotations"></param>
        /// <param name="messageFunc"></param>
        public SyntaxBuilderContext(
            SyntaxBuilderContext parent,
            string debugText,
            string defaultNamespace,
            ImmutableList<string> imports,
            SyntaxBuilder builder,
            SyntaxGenerator syntax,
            IRepositoryProvider repositories,
            ITypeInfoProvider clrTypeInfo,
            ImmutableList<object> annotations,
            Action<SyntaxBuilderMessage> messageFunc)
        {
            this.parent = parent;
            this.debugText = debugText;
            this.defaultNamespace = defaultNamespace;
            this.imports = imports ?? throw new ArgumentNullException(nameof(imports));
            this.builder = builder ?? throw new ArgumentNullException(nameof(builder));
            this.syntax = syntax ?? throw new ArgumentNullException(nameof(syntax));
            this.repositories = repositories ?? throw new ArgumentNullException(nameof(repositories));
            this.clrTypeInfo = clrTypeInfo ?? throw new ArgumentNullException(nameof(clrTypeInfo));
            this.annotations = annotations ?? throw new ArgumentNullException(nameof(annotations));
            this.messageFunc = messageFunc ?? throw new ArgumentNullException(nameof(messageFunc));
        }

        string GetDebugText()
        {
            var b = new List<string>(2);

            // get parent text if available
            if (parent?.DebugText is string p && string.IsNullOrEmpty(p) == false)
                b.Add(p);

            // add our text if available
            if (debugText != null)
                b.Add(debugText);

            return string.Join("/", b);
        }

        public string DebugText => GetDebugText();

        public string CurrentNamespace => defaultNamespace;

        public IList<string> Imports => imports;

        public IRepositoryProvider Repositories => repositories;

        public SyntaxGenerator Syntax => syntax;

        public ITypeInfoProvider Types => clrTypeInfo;

        public T Annotation<T>() => annotations.OfType<T>().FirstOrDefault();

        /// <summary>
        /// Initiates a build of the specified element, possibly with a new context.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public IEnumerable<SyntaxNode> Build(Element element)
        {
            return builder.BuildElement(this, element);
        }

        /// <summary>
        /// Generates a new context with a debug string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public IContext WithDebugText(string value)
        {
            return new SyntaxBuilderContext(
                this,
                value,
                defaultNamespace,
                imports,
                builder,
                syntax,
                repositories,
                clrTypeInfo,
                annotations,
                messageFunc);
        }

        /// <summary>
        /// Initiates a new context with modified values.
        /// </summary>
        /// <param name="import"></param>
        /// <returns></returns>
        public IContext WithImport(string import)
        {
            if (import is null)
                throw new ArgumentNullException(nameof(import));

            return new SyntaxBuilderContext(
                this,
                null,
                defaultNamespace,
                imports.Remove(import).Add(import),
                builder,
                syntax,
                repositories,
                clrTypeInfo,
                annotations,
                messageFunc);
        }

        /// <summary>
        /// Initiates a new context with modified values.
        /// </summary>
        /// <param name="defaultNamespace"></param>
        /// <returns></returns>
        public IContext WithNamespace(string defaultNamespace = null)
        {
            return new SyntaxBuilderContext(
                this,
                null,
                defaultNamespace ?? this.defaultNamespace,
                defaultNamespace != null ? imports.Add(defaultNamespace) : imports,
                builder,
                syntax,
                repositories,
                clrTypeInfo,
                annotations,
                messageFunc);
        }

        /// <summary>
        /// Initiates a new context with an annotation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="annotation"></param>
        /// <returns></returns>
        public IContext WithAnnotation<T>(T annotation)
        {
            return new SyntaxBuilderContext(
                this,
                null,
                defaultNamespace ?? this.defaultNamespace,
                imports,
                builder,
                syntax,
                repositories,
                clrTypeInfo,
                annotations.Add(annotation),
                messageFunc);
        }

        /// <summary>
        /// Adds a warning to the context.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        IContext AddMessage(SyntaxBuilderMessage message)
        {
            if (message is null)
                throw new ArgumentNullException(nameof(message));

            messageFunc(message);
            return this;
        }

        public IContext Debug(FormattableString text)
        {
            return AddMessage(new SyntaxBuilderMessage(this, SyntaxBuilderMessageSeverity.Debug, text));
        }

        public IContext Warning(FormattableString text)
        {
            return AddMessage(new SyntaxBuilderMessage(this, SyntaxBuilderMessageSeverity.Warning, text));
        }

        public IContext Error(FormattableString text)
        {
            return AddMessage(new SyntaxBuilderMessage(this, SyntaxBuilderMessageSeverity.Error, text));
        }

        public Exception Exception(FormattableString message)
        {
            return new SyntaxBuilderException(this, message.ToString());
        }

    }

}
