﻿using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Library.Model
{

    /// <summary>
    /// Interface which is pre-required to implement another interface. This node is generally using within an interface element.
    /// </summary>
    public class PrerequisiteElement : Element
    {

        public static IEnumerable<PrerequisiteElement> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<PrerequisiteElement>();
        }

        public static PrerequisiteElement Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "prerequisite" ? Populate(new PrerequisiteElement(), element) : null;
        }

        public static PrerequisiteElement Populate(PrerequisiteElement target, XElement element)
        {
            Element.Populate(target, element);
            target.Name = (string)element.Attribute("name");
            return target;
        }

        /// <summary>
        /// Name of the required interface.
        /// </summary>
        public string Name { get; set; }

    }

}
