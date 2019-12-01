using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using Gir.Model;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace Gir.CodeGen
{

    /// <summary>
    /// Provides access to known type information.
    /// </summary>
    class TypeInfoRepositorySource : ITypeInfoSource
    {

        readonly SyntaxGenerator syntax;
        readonly Lazy<Dictionary<TypeName, Element>> index;
        readonly ConcurrentDictionary<TypeName, TypeInfo> cache = new ConcurrentDictionary<TypeName, TypeInfo>();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="syntax"></param>
        /// <param name="repositories"></param>
        public TypeInfoRepositorySource(SyntaxGenerator syntax, IRepositoryProvider repositories)
        {
            this.syntax = syntax ?? throw new ArgumentNullException(nameof(syntax));

            index = new Lazy<Dictionary<TypeName, Element>>(() =>
                repositories.GetRepositories()
                    .SelectMany(i => i.Namespaces
                        .SelectMany(j => Enumerable.Empty<Element>()
                            .Concat(j.Primitives)
                            .Concat(j.Aliases)
                            .Concat(j.BitFields)
                            .Concat(j.Boxed)
                            .Concat(j.Callbacks)
                            .Concat(j.Classes)
                            .Concat(j.Enums)
                            .Concat(j.Interfaces)
                            .Concat(j.Records)
                            .Concat(j.Unions)
                            .OfType<IHasName>()
                            .Select(i => new { Name = new TypeName(j.Name, i.Name), Element = (Element)i })))
                    .ToDictionary(i => i.Name, i => i.Element));
        }

        /// <summary>
        /// Returns the CLR type information for the given GIR namespace and name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public TypeInfo Resolve(TypeName name)
        {
            return cache.GetOrAdd(name, BuildTypeInfo);
        }

        /// <summary>
        /// Generates the type info if it does not already exist.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        TypeInfo BuildTypeInfo(TypeName name)
        {
            // retrieve element from index
            if (index.Value.TryGetValue(name, out var element) == false)
                return null;

            // an alias doesn't appear as a type and is simply resolved recursively
            if (element is Alias alias)
                return Resolve(TypeName.Parse(alias.Type.Name, name.Namespace));

            // begin building new type info
            var typeInfo = new TypeInfo(name);
            typeInfo.Element = element;

            // element might come along with some CLR information specified
            if (element is IHasClrInfo hasClrInfo && hasClrInfo.ClrInfo is ClrInfo clrInfo)
            {
                if (clrInfo.Type != null)
                    typeInfo.ClrTypeExpression = syntax.DottedName(ClrTypeName.Parse(clrInfo.Type));

                if (clrInfo.NullableType != null)
                    typeInfo.ClrNullableTypeExpression = syntax.DottedName(ClrTypeName.Parse(clrInfo.NullableType));

                if (clrInfo.NullExpression != null)
                    typeInfo.ClrNullExpression = syntax.DottedName(clrInfo.NullExpression);

                if (clrInfo.MarshalerType != null)
                    typeInfo.ClrMarshalerTypeExpression = syntax.DottedName(ClrTypeName.Parse(clrInfo.MarshalerType));
            }

            // default to CLR type name based on GIR type name
            if (typeInfo.ClrTypeExpression == null)
                typeInfo.ClrTypeExpression = syntax.DottedName(new ClrTypeName(name.Namespace, name.Name));

            // fallback to a default nullable representation based on element type
            if (typeInfo.ClrNullableTypeExpression == null)
                switch (element)
                {
                    case BitField _:
                    case Enumeration _:
                    case Record _:
                        typeInfo.ClrNullableTypeExpression = syntax.NullableTypeExpression(typeInfo.ClrTypeExpression);
                        break;
                    default:
                        typeInfo.ClrNullableTypeExpression = typeInfo.ClrTypeExpression;
                        break;
                };

            // by default we simply use "null"
            if (typeInfo.ClrNullExpression == null)
                typeInfo.ClrNullExpression = syntax.NullLiteralExpression();

            return typeInfo;
        }

    }

}
