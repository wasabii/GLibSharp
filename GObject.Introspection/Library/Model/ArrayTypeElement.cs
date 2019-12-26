using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GObject.Introspection.Library.Model
{

    /// <summary>
    /// A simple type of data (as opposed to an array).
    /// </summary>
    public class ArrayTypeElement : AnyTypeElement
    {

        public static new IEnumerable<ArrayTypeElement> LoadFrom(XContainer container)
        {
            return container.Elements().Select(i => Load(i)).OfType<ArrayTypeElement>();
        }

        public static new ArrayTypeElement Load(XElement element)
        {
            return element.Name == Xmlns.Core_1_0_NS + "array" ? Populate(new ArrayTypeElement(), element) : null;
        }

        public static ArrayTypeElement Populate(ArrayTypeElement target, XElement element)
        {
            AnyTypeElement.Populate(target, element);
            target.ZeroTerminated = element.Attribute("zero-terminated").ToBool();
            target.FixedSize = (int?)element.Attribute("fixed-size");
            target.Length = (int?)element.Attribute("length");
            target.Type = AnyTypeElement.LoadFrom(element).FirstOrDefault();
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
        public AnyTypeElement Type { get; set; }

        public override string ToString()
        {
            return Name ?? CType;
        }

    }

}
