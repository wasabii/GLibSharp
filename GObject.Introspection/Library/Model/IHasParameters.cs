using System.Collections.Generic;

namespace GObject.Introspection.Library.Model
{

    public interface IHasParameters
    {

        List<IParameter> Parameters { get; set; }

    }

}
