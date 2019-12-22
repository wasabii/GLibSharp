using System;

namespace GObject.Introspection.Model
{

    /// <summary>
    /// Describes the type of qualifiers applied to a type.
    /// </summary>
    [Flags]
    public enum NativeTypeSymbolQualifier
    {

        /// <summary>
        /// No qualifier specified.
        /// </summary>
        None = 0,

        /// <summary>
        /// Native type is const.
        /// </summary>
        Const = 1,

        /// <summary>
        /// Native type is volatile.
        /// </summary>
        Volatile = 2,

    }

}