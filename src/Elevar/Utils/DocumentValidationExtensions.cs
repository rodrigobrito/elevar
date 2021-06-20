namespace Elevar.Utils
{
    public static class DocumentValidationExtensions
    {
        public static bool IsCpf(this string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return false;

            str = str.Trim()
                     .Replace(".", string.Empty)
                     .Replace("-", string.Empty);

            const int cpfSize = 11;
            if (str.Length != cpfSize) return false;

            var firstMultiplier = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            var secondMultiplier = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            var temp = str.Substring(0, 9);
            var sum = 0;

            for (int i = 0; i < 9; i++)
                sum += int.Parse(temp[i].ToString()) * firstMultiplier[i];

            var residual = sum % 11;
            if (residual < 2)
                residual = 0;
            else
                residual = cpfSize - residual;

            var digit = residual.ToString();
            temp += digit;
            sum = 0;

            for (int i = 0; i < 10; i++)
                sum += int.Parse(temp[i].ToString()) * secondMultiplier[i];

            residual = sum % cpfSize;

            residual = residual < 2 ? 0 : cpfSize - residual;

            digit += residual.ToString();
            return str.EndsWith(digit);
        }

        public static bool IsCnpj(this string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return false;

            var firstMultiplier = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            var secondMultiplier = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            str = str.Trim()
                     .Replace(".", "")
                     .Replace("-", "")
                     .Replace("/", "");

            if (str.Length != 14) return false;

            var temp = str.Substring(0, 12);
            var sum = 0;

            for (int i = 0; i < 12; i++)
                sum += int.Parse(temp[i].ToString()) * firstMultiplier[i];

            var residual = (sum % 11);
            if (residual < 2)
                residual = 0;
            else
                residual = 11 - residual;

            var digit = residual.ToString();

            temp += digit;

            sum = 0;
            for (int i = 0; i < 13; i++)
                sum += int.Parse(temp[i].ToString()) * secondMultiplier[i];

            residual = (sum % 11);
            residual = residual < 2 ? 0 : 11 - residual;
            digit += residual.ToString();

            return str.EndsWith(digit);
        }

        public static bool IsPis(this string str)
        {
            var multiplier = new int[10] { 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            if (str.Trim().Length != 11) return false;

            str = str.Trim();
            str = str.Replace("-", "").Replace(".", "").PadLeft(11, '0');

            var sum = 0;

            for (int i = 0; i < 10; i++)
                sum += int.Parse(str[i].ToString()) * multiplier[i];

            var residual = sum % 11;
            residual = residual < 2 ? 0 : 11 - residual;
            return str.EndsWith(residual.ToString());
        }
    }
}
