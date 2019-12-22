using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace GObject.Introspection.Xml
{

    static class XmlUtil
    {

        /// <summary>
        /// Converts the attribute to a boolean.
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static bool? ToBool(this XAttribute attribute)
        {
            switch ((int?)attribute)
            {
                case 0:
                    return false;
                case 1:
                    return true;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Extracts an enum from the attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static T? ToEnum<T>(this XAttribute attribute)
            where T : struct, System.Enum
        {
            return ParseEnum<T>((string)attribute);
        }

        /// <summary>
        /// Finds an enum value by the given XML value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        static T? ParseEnum<T>(string value)
            where T : struct, System.Enum
        {
            if (value == null)
                return null;

            var member = typeof(T).GetMembers().FirstOrDefault(i => i.GetCustomAttribute<XmlEnumAttribute>(false)?.Name == value) as FieldInfo;
            if (member == null)
                return null;

            return (T)System.Enum.Parse(typeof(T), member.Name);
        }

        public static List<string> ParseStringList(string value)
        {
            return value?.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)?.ToList() ?? new List<string>();
        }

    }

}
