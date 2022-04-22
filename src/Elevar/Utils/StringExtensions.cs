using System;
using System.Globalization;
using System.Text;

namespace Elevar.Utils
{
    public static class StringExtensions
    {
        public static string ToFriendlyName(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return string.Empty;
            return FormatCharacters(RemoveDiacritics(text.Trim())).Replace("--", "-").ToLower();
        }

        public static string RemoveDiacritics(this string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();
            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }
            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static string FormatCharacters(this string str)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < str.Length; i++)
            {
                if ((str[i] >= '0' && str[i] <= '9')
                    || (str[i] >= 'A' && str[i] <= 'z'
                        || (str[i] == '_')))
                {
                    sb.Append(str[i]);
                }
                else
                {
                    if (i == (str.Length - 1) && str[i] == '.') continue;
                    if (i > 0 && str[i - 1] != '-' && str[i] != '(' && str[i] != ')' && str[i] != ',' &&
                        str[i] != ';')
                        sb.Append('-');
                }
            }
            return sb.ToString();
        }

        public static string EscapeSingleQuotes(this string str)
        {
            return str.Replace("'", "\'");
        }

        public static Uri ToUri(this string text)
        {
            return string.IsNullOrWhiteSpace(text) ? null : new Uri(text);
        }

        public static Guid? ToGuid(this string text)
        {
            if (Guid.TryParse(text, out var guid)) return guid;
            return null;
        }

        /// <summary>
        /// Converts a <see cref="string"/> to lower camel case.
        /// </summary>
        /// <param name="name">The name to be converted with lower camel case.</param>
        /// <returns>The converted name.</returns>
        public static string ToLowerCamelCase(this string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return name;
            }

            if (!char.IsUpper(name[0]))
            {
                return name;
            }

            var stringBuilder = new StringBuilder();
            for (int index = 0; index < name.Length; index++)
            {
                if (index != 0 && index + 1 < name.Length && !Char.IsUpper(name[index + 1]))
                {
                    stringBuilder.Append(name.Substring(index));
                    break;
                }
                else
                {
                    stringBuilder.Append(Char.ToLower(name[index], CultureInfo.InvariantCulture));
                }
            }
            return stringBuilder.ToString();
        }
    }
}