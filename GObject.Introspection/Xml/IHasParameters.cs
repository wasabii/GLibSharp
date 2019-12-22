using System.Collections.Generic;

namespace GObject.Introspection.Xml
{

    public interface IHasParameters
    {

        List<IParameter> Parameters { get; set; }

    }

}
