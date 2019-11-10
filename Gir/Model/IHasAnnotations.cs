using System.Collections.Generic;

namespace Gir.Model
{

    public interface IHasAnnotations
    {

        List<Annotation> Annotations { get; set; }

    }

}