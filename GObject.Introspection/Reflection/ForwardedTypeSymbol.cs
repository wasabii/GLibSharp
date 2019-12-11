using System;

using GObject.Introspection.Model;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes a type symbol refering to a CLR type.
    /// </summary>
    class ForwardedTypeSymbol : TypeSymbol
    {

        readonly IHasClrInfo element;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="clrTypeName"></param>
        public ForwardedTypeSymbol(IHasClrInfo element)
        {
            this.element = element ?? throw new ArgumentNullException(nameof(element));
        }

        /// <summary>
        /// Gets the qualified CLR type name.
        /// </summary>
        public override string QualifiedName => element.ClrInfo.Type;

    }

}
