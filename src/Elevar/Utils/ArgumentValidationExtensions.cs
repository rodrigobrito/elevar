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
    }
}
