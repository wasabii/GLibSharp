namespace GObject.Introspection.Library.Model
{

    /// <summary>
    /// Indicates that the element has information.
    /// </summary>
    public interface IHasInfo : IHasDocumentation, IHasAnnotations
    {

        Info Info { get; set; }

    }

}
