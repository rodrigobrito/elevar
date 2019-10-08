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
        public static Uri ToUri(this string text)
        {
            return string.IsNullOrWhiteSpace(text) ? null : new Uri(text);
        }
    }
}
