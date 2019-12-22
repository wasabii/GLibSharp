namespace GObject.Introspection.Model
{

    /// <summary>
    /// Describes a reference to either an introspected type or a system type.
    /// </summary>
    public abstract class TypeSymbol
    {

        /// <summary>
        /// Gets the qualified CLR type name.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Returns <c>true</c> if the type symbol references an array.
        /// </summary>
        public virtual bool IsArray => false;

        /// <summary>
        /// Returns <c>true</c> if the type is blittable to the native version of the type.
        /// </summary>
        public virtual bool IsBlittable => false;

        /// <summary>
        /// Gets a string representation of the type symbol.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }

    }

}
