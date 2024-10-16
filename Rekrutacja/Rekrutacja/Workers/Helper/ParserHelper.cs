namespace Rekrutacja.Workers.Helper
{
    public static class ParserHelper
    {
        public static int StringToInt(string str)
        {
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
