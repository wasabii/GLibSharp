using System.Collections.Generic;

namespace GObject.Introspection.Model
{

    public interface IHasParameters
    {

        List<IParameter> Parameters { get; set; }

    }

}
