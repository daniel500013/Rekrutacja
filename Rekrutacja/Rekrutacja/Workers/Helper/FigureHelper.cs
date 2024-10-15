using Rekrutacja.Workers.Enums;

namespace Rekrutacja.Workers.Helper
{
    public static class FigureHelper
    {
        public static int Calculate(FigureEnum figure, double number1, double number2)
        {
            int result = 0;

            switch (figure)
            {
                case FigureEnum.Square:
                case FigureEnum.Rectangle:
                    result = (int)(number1 * number2);
                    break;
                case FigureEnum.Triangle:
                    result = (int)(number1 * number2) / 2;
                    break;

                case FigureEnum.Circle:
                    result = (int)(System.Math.PI * System.Math.Pow(number1, 2));
                    break;
            }

            return result;
        }
    }
}
