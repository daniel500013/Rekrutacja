using Rekrutacja.Workers.Enums;
using Soneta.Business.UI;
using Soneta.Produkcja;

namespace Rekrutacja.Workers.Helper
{
    public static class CalculatorHelper
    {
        public static double Calculate(OperationEnum operation, double number1, double number2)
        {
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
            }

            return result;
        }
    }
}
