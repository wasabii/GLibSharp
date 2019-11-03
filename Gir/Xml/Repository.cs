using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Gir.Xml
{

    public class Repository
    {

        public static IEnumerable<Repository> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<Repository>();
        }

        public static Repository Load(XElement element)
        {
            if (element.Name == Xmlns.Core_1_0_NS + "repository")
                return Populate(new Repository(), element);

            return null;
        }

        public static Repository Populate(Repository target, XElement element)
        {
            target.Version = (string)element.Attribute("version");
            target.CIdentifierPrefixes = XmlUtil.ParseStringList((string)element.Attribute(Xmlns.C_1_0_NS + "identifier-prefixes"));
            target.CSymbolPrefixes = XmlUtil.ParseStringList((string)element.Attribute(Xmlns.C_1_0_NS + "symbol-prefixes"));
            target.Includes = Include.LoadFrom(element).ToList();
            target.CIncludes = CInclude.LoadFrom(element).ToList();
            target.Packages = Package.LoadFrom(element).ToList();
            target.Namespaces = Namespace.LoadFrom(element).ToList();
            return target;
        }

        public string Version { get; set; }

        public List<string> CIdentifierPrefixes { get; set; }

        public List<string> CSymbolPrefixes { get; set; }

        public List<Include> Includes { get; set; }

        public List<CInclude> CIncludes { get; set; }

        public List<Package> Packages { get; set; }

        public List<Namespace> Namespaces { get; set; }

    }

}
