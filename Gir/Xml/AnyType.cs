using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Gir.Xml
{

    public abstract class AnyType
    {

        public static IEnumerable<AnyType> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<AnyType>();
        }

        public static AnyType Load(XElement element)
        {
            return (AnyType)Type.Load(element) ?? (AnyType)ArrayType.Load(element);
        }

    }

}
