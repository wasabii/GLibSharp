using System.Collections.Generic;

namespace Gir.Model
{

    public interface IHasParameters
    {

        List<IParameter> Parameters { get; set; }

    }

}
