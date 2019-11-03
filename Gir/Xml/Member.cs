﻿using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Gir.Xml
{

    /// <summary>
    /// Element defining a member of a bit field or an enumeration
    /// </summary>
    public class Member : IHasInfo
    {

        public static IEnumerable<Member> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<Member>();
        }

        public static Member Load(XElement element)
        {
            if (element.Name == Xmlns.Core_1_0_NS + "member")
                return Populate(new Member(), element);

            return null;
        }

        public static Member Populate(Member target, XElement element)
        {
            target.Info = Info.Load(element);
            target.Documentation = Documentation.Load(element);
            target.Annotations = Annotation.LoadFrom(element).ToList();
            target.Name = (string)element.Attribute("name");
            target.Value = (string)element.Attribute("value");
            target.CIdentifier = (string)element.Attribute(Xmlns.C_1_0_NS + "identifier");
            target.GLibNick = (string)element.Attribute(Xmlns.GLib_1_0_NS + "nick");
            return target;
        }

        public Info Info { get; set; }

        public Documentation Documentation { get; set; }

        public List<Annotation> Annotations { get; set; }

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