using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Library.Model
{

    public class RepositoryElement : Element
    {

        public static IEnumerable<RepositoryElement> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<RepositoryElement>();
        }

        public static RepositoryElement Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "repository" ? Populate(new RepositoryElement(), element) : null;
        }

        public static RepositoryElement Populate(RepositoryElement target, XElement element)
        {
            Element.Populate(target, element);
            target.Version = (string)element.Attribute("version");
            target.CIdentifierPrefixes = XmlUtil.ParseStringList((string)element.Attribute(Xmlns.C_1_0_NS + "identifier-prefixes"));
            target.CSymbolPrefixes = XmlUtil.ParseStringList((string)element.Attribute(Xmlns.C_1_0_NS + "symbol-prefixes"));
            target.Includes = IncludeElement.LoadFrom(element).ToList();
            target.CIncludes = CIncludeElement.LoadFrom(element).ToList();
            target.Packages = PackageElement.LoadFrom(element).ToList();
            target.Namespaces = NamespaceElement.LoadFrom(target, element).ToList();
            return target;
        }

        public string Version { get; set; }

        public List<string> CIdentifierPrefixes { get; set; }

        public List<string> CSymbolPrefixes { get; set; }

        public List<IncludeElement> Includes { get; set; }

        public List<CIncludeElement> CIncludes { get; set; }

        public List<PackageElement> Packages { get; set; }

        public List<NamespaceElement> Namespaces { get; set; }

    }

}
