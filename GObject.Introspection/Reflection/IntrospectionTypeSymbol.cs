﻿using System;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes a reference to a type available within the reflection library.
    /// </summary>
    public class IntrospectionTypeSymbol : TypeSymbol
    {

        readonly IntrospectionTypeDef typeDef;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="typeDef"></param>
        internal IntrospectionTypeSymbol(IntrospectionTypeDef typeDef)
        {
            this.typeDef = typeDef ?? throw new ArgumentNullException(nameof(typeDef));
        }

        /// <summary>
        /// Gets the introspection type that is referenced by this symbol.
        /// </summary>
        public IntrospectionType Type => typeDef.Type;

        /// <summary>
        /// Gets the qualified name of the type.
        /// </summary>
        public override string Name => typeDef.Name;

    }

}
