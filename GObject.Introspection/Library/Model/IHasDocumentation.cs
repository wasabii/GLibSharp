namespace GObject.Introspection.Library.Model
{

    /// <summary>
    /// Indicates that an element has documentation.
    /// </summary>
    public interface IHasDocumentation
    {

        /// <summary>
        /// Gets the documentation for the element.
        /// </summary>
        Documentation Documentation { get; set; }

    }

}