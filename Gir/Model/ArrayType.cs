using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Gir.Model
{

    /// <summary>
    /// A simple type of data (as opposed to an array).
    /// </summary>
    public class ArrayType : AnyType
    {

        public static new IEnumerable<ArrayType> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<ArrayType>();
        }

        public static new ArrayType Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "array" ? Populate(new ArrayType(), element) : null;
        }

        public static ArrayType Populate(ArrayType target, XElement element)
        {
            AnyType.Populate(target, element);
            target.ZeroTerminated = element.Attribute("zero-terminated").ToBool();
            target.FixedSize = (int?)element.Attribute("fixed-size");
            target.Length = (int?)element.Attribute("length");
            target.Type = AnyType.LoadFrom(element).FirstOrDefault();
            return target;
        }

        /// <summary>
        /// True if the last element of the array is zero. For example, in an array of pointers, the last pointer would be NULL.
        /// </summary>
        public bool? ZeroTerminated { get; set; }

        /// <summary>
        /// Size of an array of predetermined fixed size. For example a C array declared as char arr[5].
        /// </summary>
        public int? FixedSize { get; set; }

        /// <summary>
        /// 0-based index of parameter element that specifies the length of the array.
        /// </summary>
        public int? Length { get; set; }

        /// <summary>
        /// Type of the values contained in the array.
        /// </summary>
        public AnyType Type { get; set; }

        public override string ToString()
        {
            return Name ?? CType;
        }

    }

}
