using System;

namespace Elevar.Utils
{
    public static class ArgumentValidationExtensions
    {
        public static T ThrowIfNull<T>(this T o, string paramName) where T : class
        {
            if (o == null)
                throw new ArgumentNullException(paramName);

            return o;
        }

        public static Guid ThrowIfEmpty(this Guid o, string paramName)
        {
            if (o == Guid.Empty)
                throw new ArgumentNullException(paramName);

            return o;
        }

        public static string ThrowIfNullOrEmpty(this string str, string paramName)
        {
            if (string.IsNullOrWhiteSpace(str))
                throw new ArgumentNullException(paramName);

            return str;
        }
    }
}