﻿using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Model
{

    /// <summary>
    /// Element defining a bit field (as in C).
    /// </summary>
    public abstract class Flag : Element, IHasName, IHasInfo, IHasClrInfo
    {

        public static IEnumerable<Flag> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<Flag>();
        }

        public static Flag Load(XElement element)
        {
            return (Flag)Enumeration.Load(element) ?? (Flag)BitField.Load(element);
        }

        public static Flag Populate(Flag target, XElement element)
        {
            Element.Populate(target, element);
            target.Info = Info.Load(element);
            target.Documentation = Documentation.Load(element);
            target.Annotations = Annotation.LoadFrom(element).ToList();
            target.Name = (string)element.Attribute("name");
            target.CType = (string)element.Attribute(Xmlns.C_1_0_NS + "type");
            target.GLibTypeName = (string)element.Attribute(Xmlns.GLib_1_0_NS + "type-name");
            target.GLibGetType = (string)element.Attribute(Xmlns.GLib_1_0_NS + "get-type");
            target.Members = Member.LoadFrom(element).ToList();
            target.Functions = Function.LoadFrom(element).ToList();
            target.ClrInfo = ClrInfo.Load(element);
            return target;
        }

        public Info Info { get; set; }

        public Documentation Documentation { get; set; }

        public List<Annotation> Annotations { get; set; }

        /// <summary>
        /// Name of the bit field.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Corresponding C type of the bit field type.
        /// </summary>
        public string CType { get; set; }

        /// <summary>
        /// GObject compatible type name.
        /// </summary>
        public string GLibTypeName { get; set; }

        /// <summary>
        /// Function to retrieve the GObject compatible type of the element.
        /// </summary>
        public string GLibGetType { get; set; }

        public List<Member> Members { get; set; }

        public List<Function> Functions { get; set; }

        public ClrInfo ClrInfo { get; set; }

        public override string ToString()
        {
            return Name ?? GLibTypeName ?? CType;
        }

    }

}