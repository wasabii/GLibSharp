﻿using System.Xml.Serialization;

namespace GObject.Introspection.Library.Model
{

    public enum When
    {

        [XmlEnum("first")]
        First,

        [XmlEnum("last")]
        Last,

        [XmlEnum("cleanup")]
        Cleanup

    }

}
