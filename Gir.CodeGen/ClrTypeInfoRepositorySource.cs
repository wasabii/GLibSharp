using System;
using System.Collections.Concurrent;
using System.Linq;

using Gir.Model;

namespace Gir.CodeGen
{

    /// <summary>
    /// Provides access to CLR type information sourced from GIR-extensions, or generated on the fly.
    /// </summary>
    class ClrTypeInfoRepositorySource : IClrTypeInfoSource
    {

        readonly IRepositoryProvider repositories;
        readonly ConcurrentDictionary<GirTypeName, ClrTypeInfo> cache = new ConcurrentDictionary<GirTypeName, ClrTypeInfo>();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public ClrTypeInfoRepositorySource(IRepositoryProvider resolver)
        {
            this.repositories = resolver ?? throw new ArgumentNullException(nameof(resolver));
        }

        /// <summary>
        /// Returns the CLR type information for the given GIR namespace and name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ClrTypeInfo Resolve(GirTypeName name)
        {
            return cache.GetOrAdd(name, BuildClrTypeInfo);
        }

        /// <summary>
        /// Generates the CLR type info if it does not already exist.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        ClrTypeInfo BuildClrTypeInfo(GirTypeName name)
        {
            var element = repositories.GetRepositories()
                .SelectMany(i => i.Namespaces
                    .Where(j => j.Name == name.Namespace))
                .SelectMany(i => Enumerable.Empty<Element>()
                    .Concat(i.Primitives)
                    .Concat(i.Aliases)
                    .Concat(i.BitFields)
                    .Concat(i.Boxed)
                    .Concat(i.Callbacks)
                    .Concat(i.Classes)
                    .Concat(i.Enums)
                    .Concat(i.Interfaces)
                    .Concat(i.Records)
                    .Concat(i.Unions))
                .Cast<IHasName>()
                .Where(i => i.Name == name.Name)
                .Cast<Element>()
                .FirstOrDefault();
            if (element == null)
                return null;

            // an alias doesn't appear as a CLR type, and is simply resolved recursively
            if (element is Alias alias)
                return Resolve(GirTypeName.Parse(alias.Type.Name, name.Namespace));

            // name of the resolved element
            var girTypeName = element is IHasName named ? named.Name : null;
            if (girTypeName is null)
                throw new InvalidOperationException("Could not obtain type name from element.");

            // element might come along with some CLR information specified
            var clrTypeInfo = element is IHasClrInfo hasClrInfo ? hasClrInfo.ClrInfo : null;

            // derive CLR type name to generate
            var clrTypeName = clrTypeInfo?.Name;
            if (clrTypeName == null && girTypeName != null)
                clrTypeName = girTypeName;
            if (clrTypeName == null)
                throw new InvalidOperationException("Unable to determine name for element.");

            var girQTypeName = new GirTypeName(name.Namespace, girTypeName);
            var clrQTypeName = ClrTypeName.Parse(clrTypeName, girQTypeName.Namespace);

            return new ClrTypeInfo(girQTypeName, clrQTypeName)
            {
                Element = element,
                ClrMarshalerTypeName = clrTypeInfo?.Marshaler != null ? ClrTypeName.Parse(clrTypeInfo.Marshaler) : (ClrTypeName?)null,
            };
        }

    }

}
