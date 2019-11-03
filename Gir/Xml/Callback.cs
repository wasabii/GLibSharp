using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Gir.Xml
{

    public class Callback : Callable, IHasInfo, IHasParameters, IHasReturnValue
    {

        public static new IEnumerable<Callback> LoadFrom(XContainer container)
        {
            return Callable.LoadFrom(container).OfType<Callback>();
        }

        public static new Callback Load(XElement element)
        {
            if (element.Name == Xmlns.Core_1_0_NS + "callback")
                return Populate(new Callback(), element);

            return null;
        }

        public static Callback Populate(Callback target, XElement element)
        {
            Callable.Populate(target, element);
            target.Documentation = Documentation.Load(element);
            target.Info = Info.Load(element);
            target.Name = (string)element.Attribute("name");
            target.CType = (string)element.Attribute(Xmlns.C_1_0_NS + "type");
            target.Throws = (int?)element.Attribute("throws") == 1;
            return target;
        }

        public Info Info { get; set; }

        public Documentation Documentation { get; set; }

        public List<Annotation> Annotations { get; set; }

        public string Name { get; set; }

        public string CType { get; set; }

        public bool Throws { get; set; }

        public override string ToString()
        {
            return Name ?? CType;
        }

    }

}
