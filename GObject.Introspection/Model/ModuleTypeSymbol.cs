using System;

namespace GObject.Introspection.Model
{

    /// <summary>
    /// Describes a reference to a type available within the reflection library.
    /// </summary>
    public class ModuleTypeSymbol : TypeSymbol
    {

        readonly TypeDef typeDef;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="typeDef"></param>
        internal ModuleTypeSymbol(TypeDef typeDef)
        {
            this.typeDef = typeDef ?? throw new ArgumentNullException(nameof(typeDef));
        }

        /// <summary>
        /// Gets the introspection type that is referenced by this symbol.
        /// </summary>
        public Type Type => typeDef.Type;

        /// <summary>
        /// Gets the qualified name of the type.
        /// </summary>
        public override string Name => typeDef.Name;

    }

}
