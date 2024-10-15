using Rekrutacja.Extensions;
using Rekrutacja.Workers.Enums;
using System;

namespace Rekrutacja.Workers.Helper
{
    public static class FigureHelper
    {
        public static int Calculate(FigureEnum figure, double number1, double number2)
        {
            if (number1.IsNegativeOrZero() || number1.IsNegativeOrZero())
                throw new ArgumentOutOfRangeException("Długość: A, B", "Wymiary figury muszą być dodatnie");

            if (figure == FigureEnum.Square && number1 != number2)
                throw new ArgumentException("Dla kwadratu obie długości muszą być równe", "Długość: A, B");

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

                default:
                    throw new ArgumentException($"Nie znaleziono figury: {figure}", nameof(figure));
            }

            return result;
        }
    }
}
