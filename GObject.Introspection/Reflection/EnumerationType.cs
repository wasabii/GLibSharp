using System;

using GObject.Introspection.Model;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes a flag type generated from an enumeration.
    /// </summary>
    class EnumerationType : FlagType
    {

        readonly Enumeration enumeration;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="enumeration"></param>
        public EnumerationType(IntrospectionContext context, Enumeration enumeration) :
            base(context, enumeration)
        {
            this.enumeration = enumeration ?? throw new ArgumentNullException(nameof(enumeration));
        }

    }

}
