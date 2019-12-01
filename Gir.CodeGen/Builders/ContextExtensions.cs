using System;

namespace Gir.CodeGen.Builders
{

    /// <summary>
    /// Provides extensions for working with the current context.
    /// </summary>
    public static class ContextExtensions
    {

        /// <summary>
        /// Resolves the type information given a name based on the current context.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static TypeInfo ResolveTypeInfo(this IContext context, string name)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            // some well known type names
            if (name == "" || name == "none")
                return null;

            // type name might be qualified initially
            if (TypeName.IsQualified(name))
                return context.Types.Resolve(TypeName.Parse(name));

            // check the imported namespaces in reverse order
            for (var i = context.Imports.Count - 1; i >= 0; i--)
            {
                var qn = new TypeName(context.Imports[i], name);
                if (ResolveTypeInfo(context, qn) is TypeInfo ti)
                    return ti;
            }

            return null;
        }

    }

}
