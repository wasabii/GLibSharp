using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace Gir.Xml
{

    static class XmlUtil
    {

        /// <summary>
        /// Finds an enum value by the given XML value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T? ParseEnum<T>(string value)
            where T : struct, System.Enum
        {
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
