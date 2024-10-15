namespace Rekrutacja.Extensions
{
    public static class NumberExtensions
    {
        public static bool IsPositive(this double number)
        {
            return number > 0;
        }

        public static bool IsNegativeOrZero(this double number)
        {
            return number <= 0;
        }
    }
}
