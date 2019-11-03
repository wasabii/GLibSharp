using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Gir.Xml
{

    /// <summary>
    /// A simple type of data (as opposed to an array).
    /// </summary>
    public class ArrayType : AnyType
    {

        public static new IEnumerable<ArrayType> LoadFrom(XContainer container)
        {
            return AnyType.LoadFrom(container).OfType<ArrayType>();
        }

        public static new ArrayType Load(XElement element)
        {
            if (element.Name == Xmlns.Core_1_0_NS + "array")
                return Populate(new ArrayType(), element);

            return null;
        }

        public static ArrayType Populate(ArrayType target, XElement element)
        {
            target.Name = (string)element.Attribute("name");
            target.ZeroTerminated = (int?)element.Attribute("zero-terminated") == 1;
            target.FixedSize = (int?)element.Attribute("fixed-size");
            target.Introspectable = (int?)element.Attribute("introspectable") != 0;
            target.Length = (int?)element.Attribute("length");
            target.CType = (string)element.Attribute(Xmlns.C_1_0_NS + "type");
            return target;
        }

        /// <summary>
        /// Name of the type.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// True if the last element of the array is zero. For example, in an array of pointers, the last pointer would be NULL.
        /// </summary>
        public bool? ZeroTerminated { get; set; }

        /// <summary>
        /// Size of an array of predetermined fixed size. For example a C array declared as char arr[5].
        /// </summary>
        public int? FixedSize { get; set; }

        /// <summary>
        /// Binary attribute which is false if the element is not introspectable.
        /// </summary>
        public bool? Introspectable { get; set; }

        /// <summary>
        /// 0-based index of parameter element that specifies the length of the array.
        /// </summary>
        public int? Length { get; set; }

        /// <summary>
        /// The C representation of hte type.
        /// </summary>
        public string CType { get; set; }

        /// <summary>
        /// Type of the values contained in the array.
        /// </summary>
        public Type Type { get; set; }

        public override string ToString()
        {
            return Name ?? CType;
        }

    }

}
