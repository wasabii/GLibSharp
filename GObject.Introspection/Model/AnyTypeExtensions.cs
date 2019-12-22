using System;

using GObject.Introspection.Xml;

namespace GObject.Introspection.Model
{

    /// <summary>
    /// Provides some extension methods for working with <see cref="AnyTypeElement"/> elements.
    /// </summary>
    static class AnyTypeExtensions
    {

        /// <summary>
        /// Looks up the symbol describing the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static TypeSymbol ToSymbol(this Xml.TypeElement type, Context context)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            // default to object for unknown types
            if (type == null || type.Name == "none")
                return null;

            // type name must be provided
            if (type.Name == null)
                throw new ModuleException($"Could not find type name for type reference.");

            var symbol = context.ResolveSymbol(type.Name);
            if (symbol == null)
                throw new ModuleException($"Could not resolve type reference {type.Name}.");

            return symbol;
        }

        /// <summary>
        /// Translates the given <see cref="AnyTypeElement"/> element into a <see cref="TypeSymbol"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static TypeSpec ToSpec(this AnyTypeElement type, Context context)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            return type switch
            {
                GObject.Introspection.Xml.TypeElement t => GetTypeSpec(context, t),
                ArrayTypeElement a => GetTypeSpec(context, a),
                _ => throw new InvalidOperationException(),
            };
        }

        /// <summary>
        /// Translates the given <see cref="Xml.TypeElement"/> element into a <see cref="TypeSpec"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        static TypeSpec GetTypeSpec(Context context, Xml.TypeElement type)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            var typeName = type.Name;
            if (typeName == "" || typeName == "none")
                typeName = null;

            var nativeTypeName = type.CType;
            if (nativeTypeName == "" || nativeTypeName == "none")
                nativeTypeName = null;

            if (typeName == null && nativeTypeName == null)
                throw new InvalidOperationException("Neither type name nor native type specified.");

            var symbol = typeName != null ? context.ResolveSymbol(typeName) : null;
            if (symbol == null && typeName != null)
                throw new ModuleException($"Could not resolve type symbol {typeName}.");

            var nativeSymbol = nativeTypeName != null ? NativeTypeSymbol.Parse(nativeTypeName, context.ResolveNativeSymbol) : null;
            if (nativeSymbol == null && nativeTypeName != null)
                throw new ModuleException($"Could not resolve native type symbol {nativeTypeName}.");

            return new TypeSpec(symbol, nativeSymbol);
        }

        /// <summary>
        /// Translates the given <see cref="ArrayTypeElement"/> element into a <see cref="TypeSpec"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        static TypeSpec GetTypeSpec(Context context, ArrayTypeElement type)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            var typeName = type.Name;
            if (typeName == "" || typeName == "none")
                typeName = null;

            var nativeTypeName = type.CType;
            if (nativeTypeName == "" || nativeTypeName == "none")
                nativeTypeName = null;

            return new ArrayTypeSpec(
                typeName != null ? context.ResolveSymbol(typeName) : null,
                nativeTypeName != null ? context.ResolveNativeSymbol(nativeTypeName) : null,
                ToSpec(type.Type, context),
                type.FixedSize);
        }

    }

}
