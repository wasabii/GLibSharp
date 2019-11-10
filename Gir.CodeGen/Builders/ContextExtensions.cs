using System;
using System.Linq;

using Gir.Model;

namespace Gir.CodeGen.Builders
{

    /// <summary>
    /// Provides extensions for working with the current context.
    /// </summary>
    public static class ContextExtensions
    {

        /// <summary>
        /// Resolves the given type name to an element based on the current context.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static GirTypeName? ResolveTypeName(this IContext context, string name)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            // some well known type names
            if (name == "" ||
                name == "none")
                return null;

            // type name might be qualified initially
            if (GirTypeName.IsQualified(name))
                return GirTypeName.Parse(name);

            // check the imported namespaces in reverse order
            for (var i = context.Imports.Count - 1; i >= 0; i--)
            {
                var qn = new GirTypeName(context.Imports[i], name);
                if (ResolveTypeInfo(context, qn) is Element)
                    return qn;
            }

            return null;
        }

        /// <summary>
        /// Gets the GIR element given the specified resolved type name.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Element ResolveTypeInfo(this IContext context, GirTypeName name)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            return context.Repositories.GetRepositories()
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
        }

        /// <summary>
        /// Resolves the CLR type name for the given GIR type name within the context.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ClrTypeInfo? ResolveClrTypeInfo(this IContext context, GirTypeName name)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            return context.ClrTypeInfo.Resolve(name);
        }

        /// <summary>
        /// Resolves the CLR type name for the given GIR type name within the context.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ClrTypeName? ResolveClrTypeName(this IContext context, GirTypeName name)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            return context.ResolveClrTypeInfo(name)?.ClrTypeName;
        }

    }

}
