using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Gir.Model;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace Gir.CodeGen
{

    /// <summary>
    /// Provides a context object for this builder.
    /// </summary>
    class SymbolBuilderContext : IContext
    {

        readonly string defaultNamespace;
        readonly ImmutableList<string> imports;
        readonly SyntaxBuilder builder;
        readonly SyntaxGenerator syntax;
        readonly IRepositoryProvider repositories;
        readonly IClrTypeInfoProvider clrTypeInfo;
        readonly ImmutableList<object> annotations;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="defaultNamespace"></param>
        /// <param name="builder"></param>
        /// <param name="syntax"></param>
        /// <param name="repositories"></param>
        /// <param name="clrTypeInfo"></param>
        public SymbolBuilderContext(
            string defaultNamespace,
            ImmutableList<string> imports,
            SyntaxBuilder builder,
            SyntaxGenerator syntax,
            IRepositoryProvider repositories,
            IClrTypeInfoProvider clrTypeInfo,
            ImmutableList<object> annotations)
        {
            this.defaultNamespace = defaultNamespace;
            this.imports = imports ?? throw new ArgumentNullException(nameof(imports));
            this.builder = builder ?? throw new ArgumentNullException(nameof(builder));
            this.syntax = syntax ?? throw new ArgumentNullException(nameof(syntax));
            this.repositories = repositories ?? throw new ArgumentNullException(nameof(repositories));
            this.clrTypeInfo = clrTypeInfo ?? throw new ArgumentNullException(nameof(clrTypeInfo));
            this.annotations = annotations ?? throw new ArgumentNullException(nameof(annotations));
        }

        public string CurrentNamespace => defaultNamespace;

        public IList<string> Imports => imports;

        public IRepositoryProvider Repositories => repositories;

        public SyntaxGenerator Syntax => syntax;

        public IClrTypeInfoProvider ClrTypeInfo => clrTypeInfo;

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
        /// Initiates a new context with modified values.
        /// </summary>
        /// <param name="import"></param>
        /// <returns></returns>
        public IContext WithImport(string import)
        {
            if (import is null)
                throw new ArgumentNullException(nameof(import));

            return new SymbolBuilderContext(
                defaultNamespace,
                imports.Add(import),
                builder,
                syntax,
                repositories,
                clrTypeInfo,
                annotations);
        }

        /// <summary>
        /// Initiates a new context with modified values.
        /// </summary>
        /// <param name="defaultNamespace"></param>
        /// <returns></returns>
        public IContext WithNamespace(string defaultNamespace = null)
        {
            return new SymbolBuilderContext(
                defaultNamespace ?? this.defaultNamespace,
                defaultNamespace != null ? imports.Add(defaultNamespace) : imports,
                builder,
                syntax,
                repositories,
                clrTypeInfo,
                annotations);
        }

        /// <summary>
        /// Initiates a new context with an annotation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="annotation"></param>
        /// <returns></returns>
        public IContext WithAnnotation<T>(T annotation)
        {
            return new SymbolBuilderContext(
                defaultNamespace ?? this.defaultNamespace,
                imports,
                builder,
                syntax,
                repositories,
                clrTypeInfo,
                annotations.Add(annotation));
        }

    }

}
