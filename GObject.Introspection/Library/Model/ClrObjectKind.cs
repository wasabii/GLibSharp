namespace GObject.Introspection.Library.Model
{

    /// <summary>
    /// Specifies the type of object that should be produced by the element.
    /// </summary>
    public enum ClrObjectKind
    {

        /// <summary>
        /// The kind should be determined automatically.
        /// </summary>
        Auto,

        /// <summary>
        /// The kind of generated object should be a class.
        /// </summary>
        Class,

        /// <summary>
        /// The kind of generated object should be a value type.
        /// </summary>
        Value,

    }

}
