using System;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes a reference to a type available within the reflection library.
    /// </summary>
    public class IntrospectionTypeSymbol : TypeSymbol
    {

        readonly IntrospectionType type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        public IntrospectionTypeSymbol(IntrospectionType type)
        {
            this.type = type ?? throw new ArgumentNullException(nameof(type));
        }

        /// <summary>
        /// Gets the introspection type that is referenced by this symbol.
        /// </summary>
        public IntrospectionType Type => type;

        /// <summary>
        /// Gets the qualified name of the type.
        /// </summary>
        public override string QualifiedName => type.QualifiedName;

    }

}
