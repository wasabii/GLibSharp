using System;

using GObject.Introspection.Model;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Provides some extension methods for working with <see cref="AnyType"/> elements.
    /// </summary>
    public static class AnyTypeExtensions
    {

        /// <summary>
        /// Translates the given <see cref="AnyType"/> element into a <see cref="TypeSymbol"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static TypeSymbol ToSymbol(this AnyType type, IntrospectionContext context)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            return type switch
            {
                GObject.Introspection.Model.Type t => GetTypeSymbol(context, t),
                ArrayType a => GetTypeSymbol(context, a),
                _ => throw new InvalidOperationException(),
            };
        }

        /// <summary>
        /// Translates the given <see cref="Type"/> element into a <see cref="TypeSymbol"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        static TypeSymbol GetTypeSymbol(IntrospectionContext context, Model.Type type)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            // default to object for unknown types
            if (type == null || type.Name == "none")
                return null;

            // type name must be provided
            if (type.Name == null)
                throw new IntrospectionException($"Could not find type name for type reference.");

            var symbol = context.ResolveSymbol(type.Name);
            if (symbol == null)
                throw new IntrospectionException($"Could not resolve type reference {type.Name}.");

            return symbol;
        }

        /// <summary>
        /// Translates the given <see cref="ArrayType"/> element into a <see cref="TypeSymbol"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        static TypeSymbol GetTypeSymbol(IntrospectionContext context, ArrayType type)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            return new ArrayTypeSymbol(type.Type.ToSymbol(context));
        }

    }

}
