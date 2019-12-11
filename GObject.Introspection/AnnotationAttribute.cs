using System;

namespace GObject.Introspection
{

    public class AnnotationAttribute : Attribute
    {

        public string Name { get; set; }

        public string Value { get; set; }

    }

}
