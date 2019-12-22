using System;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes a reference to a type available within the introspection library.
    /// </summary>
    public class IntrospectionNativeTypeSymbol : NativeTypeSymbol
    {

        readonly IntrospectionTypeDef typeDef;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="typeDef"></param>
        internal IntrospectionNativeTypeSymbol(IntrospectionTypeDef typeDef)
        {
            this.typeDef = typeDef ?? throw new ArgumentNullException(nameof(typeDef));
        }

        /// <summary>
        /// Gets the introspection type that is referenced by this symbol.
        /// </summary>
        public IntrospectionType Type => typeDef.Type;

        /// <summary>
        /// Gets the name of the type.
        /// </summary>
        public override string Name => typeDef.NativeName;

    }

}
