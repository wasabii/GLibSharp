using System;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes a type symbol refering to a CLR type.
    /// </summary>
    class ForwardedTypeSymbol : TypeSymbol
    {

        readonly string typeName;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="typeName"></param>
        public ForwardedTypeSymbol(string typeName)
        {
            this.typeName = typeName ?? throw new ArgumentNullException(nameof(typeName));
        }

        /// <summary>
        /// Gets the qualified CLR type name.
        /// </summary>
        public override string QualifiedName => typeName;

    }

}
