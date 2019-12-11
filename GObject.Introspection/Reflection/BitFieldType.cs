using System;

using GObject.Introspection.Model;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes a flag type generated from a bitfield.
    /// </summary>
    class BitFieldType : FlagType
    {

        readonly BitField bitfield;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bitfield"></param>
        public BitFieldType(IntrospectionContext context, BitField bitfield) :
            base(context, bitfield)
        {
            this.bitfield = bitfield ?? throw new ArgumentNullException(nameof(bitfield));
        }

    }

}
