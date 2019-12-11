using System.Collections.Generic;

namespace GObject.Introspection.Model
{

    public interface IHasAnnotations
    {

        List<Annotation> Annotations { get; set; }

    }

}