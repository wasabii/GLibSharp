namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes a reference to either an introspected type or a system type.
    /// </summary>
    public abstract class TypeSymbol
    {

        /// <summary>
        /// Gets the qualified CLR type name.
        /// </summary>
        public abstract string QualifiedName { get; }

        /// <summary>
        /// Returns <c>true</c> if the type symbol references an array.
        /// </summary>
        public virtual bool IsArray => false;

        /// <summary>
        /// Gets a string representation of the type symbol.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return QualifiedName;
        }

    }

}
