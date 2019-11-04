namespace Gir.Xml
{

    public interface IParameter : IHasDocumentation
    {

        string Name { get; set; }

        bool? Nullable { get; set; }

        bool? AllowNone { get; set; }

        ParameterDirection? Direction { get; set; }

        bool? CallerAllocates { get; set; }

        TransferOwnership? TransferOwnership { get; set; }

    }

}
