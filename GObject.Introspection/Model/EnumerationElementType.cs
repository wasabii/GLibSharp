using System;

using GObject.Introspection.Xml;

namespace GObject.Introspection.Model
{

    /// <summary>
    /// Describes a flag type generated from an enumeration.
    /// </summary>
    class EnumerationElementType : FlagElementType
    {

        readonly EnumerationElement enumeration;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="enumeration"></param>
        public EnumerationElementType(Context context, EnumerationElement enumeration) :
            base(context, enumeration)
        {
            this.enumeration = enumeration ?? throw new ArgumentNullException(nameof(enumeration));
        }

    }

}
