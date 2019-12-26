using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Library.Model
{

    /// <summary>
    /// Element defining a member of a bit field or an enumeration
    /// </summary>
    public class MemberElement : Element, IHasInfo
    {

        public static IEnumerable<MemberElement> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<MemberElement>();
        }

        public static MemberElement Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "member" ? Populate(new MemberElement(), element) : null;
        }

        public static MemberElement Populate(MemberElement target, XElement element)
        {
            Element.Populate(target, element);
            target.Info = Info.Load(element);
            target.Documentation = Documentation.Load(element);
            target.Annotations = AnnotationElement.LoadFrom(element).ToList();
            target.Name = (string)element.Attribute("name");
            target.Value = (string)element.Attribute("value");
            target.CIdentifier = (string)element.Attribute(Xmlns.C_1_0_NS + "identifier");
            target.GLibNick = (string)element.Attribute(Xmlns.GLib_1_0_NS + "nick");
            return target;
        }

        public Info Info { get; set; }

        public Documentation Documentation { get; set; }

        public List<AnnotationElement> Annotations { get; set; }

        /// <summary>
        /// Name of the member.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Value of the member.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Corresponding C type of the member.
        /// </summary>
        public string CIdentifier { get; set; }

        /// <summary>
        /// Short nickname of the member.
        /// </summary>
        public string GLibNick { get; set; }

        public override string ToString()
        {
            return Name ?? GLibNick ?? CIdentifier;
        }

    }

}
