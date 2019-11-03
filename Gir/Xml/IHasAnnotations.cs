using System.Collections.Generic;

namespace Gir.Xml
{

    public interface IHasAnnotations
    {

        List<Annotation> Annotations { get; set; }

    }

}