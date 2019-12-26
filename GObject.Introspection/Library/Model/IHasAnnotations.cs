using System.Collections.Generic;

namespace GObject.Introspection.Library.Model
{

    public interface IHasAnnotations
    {

        List<AnnotationElement> Annotations { get; set; }

    }

}