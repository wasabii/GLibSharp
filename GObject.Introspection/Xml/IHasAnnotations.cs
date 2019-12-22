using System.Collections.Generic;

namespace GObject.Introspection.Xml
{

    public interface IHasAnnotations
    {

        List<AnnotationElement> Annotations { get; set; }

    }

}