using System;
using System.Linq;

namespace Rekrutacja.Workers.Helper
{
    public static class ParserHelper
    {
        public static int StringToInt(string str)
        {
            #region Validation
            if (string.IsNullOrEmpty(str))
                throw new ArgumentException("Podany ciąg znaków jest pusty");

            if (str == "-")
                throw new FormatException("Podano sam znak minus bez liczby");

            char[] numberChars = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            
            for (int i = 0; i < str.Length; i++)
            {
                if (i == 0 && str[i] == '-')
                    continue;
                
                if (!numberChars.Contains(str[i]))
                    throw new FormatException($"Niedozwolony znak '{str[i]}'. Dozwolone są tylko cyfry i znak minus na początku.");
            }
            #endregion

            bool isNegative = false;

            if (str.StartsWith("-"))
            {
                isNegative = true;
                str = str.Substring(1);
            }

            int answer = 0;
            int factor = 1;

            for (int i = str.Length - 1; i >= 0; i--)
            {
                char c = str[i];

                int value = c - '0';
                answer += value * factor;

                factor *= 10;
            }

            if (isNegative)
            {
                answer *= -1;
            }

            return answer;
        }
    }
}
