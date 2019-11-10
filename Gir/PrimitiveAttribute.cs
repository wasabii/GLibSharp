using System;

namespace Gir
{

    /// <summary>
    /// Describes a primitive.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class PrimitiveAttribute : Attribute
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="clrType"></param>
        public PrimitiveAttribute(string name, Type clrType)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            ClrType = clrType ?? throw new ArgumentNullException(nameof(clrType));
        }

        /// <summary>
        /// Name of the primitive.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Underlying C identifier of the type.
        /// </summary>
        public string CType { get; set; }

        /// <summary>
        /// Potential GType name of the type.
        /// </summary>
        public string GLibTypeName { get; set; }

        /// <summary>
        /// Name of the associated CLR type to map primitive to.
        /// </summary>
        public Type ClrType { get; set; }

        /// <summary>
        /// Type of the custom marshaler to use for the type.
        /// </summary>
        public Type ClrMarshalerType { get; set; }

    }

}
