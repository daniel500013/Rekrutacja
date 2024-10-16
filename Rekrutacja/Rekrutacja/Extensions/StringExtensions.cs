using System.Linq;

namespace Rekrutacja.Extensions
{
    public static class StringExtensions
    {
        public static bool ContainsNonNumeric(this string str)
        {
            char[] numberChars = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

            if (string.IsNullOrEmpty(str))
                return true;

            if (str == "-")
                return true;

            for (int i = 0; i < str.Length; i++)
            {
                if (i == 0 && str[i] == '-')
                    continue;

                if (!numberChars.Contains(str[i]))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
