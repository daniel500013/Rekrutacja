using Rekrutacja.Workers.Enums;
using Soneta.Business.UI;
using Soneta.Produkcja;
using System;

namespace Rekrutacja.Workers.Helper
{
    public static class CalculatorHelper
    {
        public static double Calculate(OperationEnum operation, double number1, double number2)
        {
            if (operation == OperationEnum.Division && number2 == 0)
                throw new DivideByZeroException("Nie można dzielić przez 0");

            double result = 0;

            switch (operation)
            {
                case OperationEnum.Add:
                    result = number1 + number2;
                    break;

                case OperationEnum.Subtraction:
                    result = number1 - number2;
                    break;

                case OperationEnum.Multiplication:
                    result = number1 * number2;
                    break;

                case OperationEnum.Division:
                    result = number1 / number2;
                    break;

                default:
                    throw new ArgumentException($"Nie znaleziono operacji");
            }

            return result;
        }
    }
}
