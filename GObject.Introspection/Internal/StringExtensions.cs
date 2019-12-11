using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GObject.Introspection.Internal
{

    static class StringExtensions
    {

        /// <summary>
        /// Converts the given string to PascalCase on word boundaries.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToPascalCase(this string value)
        {
            return string.Join("", ToWordList(value).Select(i => i.Length > 0 ? char.ToUpper(i[0]) + i.Substring(1).ToLower() : i));
        }

        /// <summary>
        /// Converts the given string to CamelCase on word boundaries.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToCamelCase(this string value)
        {
            return string.Join("", ToWordList(value)
                .Select((i, j) => j > 0 && i.Length > 0 ? char.ToUpper(i[0]) + i.Substring(1).ToLower() : i));
        }

        static IEnumerable<string> ToWordList(string value)
        {
            const string BOUNDARIES = " -_.";

            var word = new StringBuilder();

            for (var i = 0; i < value.Length; i++)
            {
                var c = value[i];
                if (BOUNDARIES.IndexOf(c) > -1)
                {
                    yield return word.ToString();
                    word.Clear();
                }
                else
                {
                    if (char.IsDigit(c) || char.IsUpper(c))
                    {
                        var hasPrev = i > 0;
                        var hasNext = i < value.Length - 1;
                        var prevLowerCase = hasPrev && char.IsLower(value[i - 1]);
                        var prevDigitUpperCase = hasPrev && char.IsDigit(value[i - 1]) && char.IsUpper(value[i - 1]);
                        var nextLowerCase = hasNext && char.IsLower(value[i + 1]);
                        if (prevLowerCase || (prevDigitUpperCase && nextLowerCase))
                        {
                            yield return word.ToString();
                            word.Clear();
                        }
                    }

                    word.Append(c);
                }
            }

            yield return word.ToString();
        }

    }

}
