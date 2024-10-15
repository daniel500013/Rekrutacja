using NUnit.Framework;
using Rekrutacja.Workers.Enums;
using Rekrutacja.Workers.Helper;
using System;

namespace Rekrutacja.Tests
{
    [TestFixture]
    public class CalculatorHelperTests
    {
        [TestCase(OperationEnum.Add, 5, 3, 8)]
        [TestCase(OperationEnum.Add, -5, 3, -2)]
        [TestCase(OperationEnum.Add, 0, 0, 0)]
        [TestCase(OperationEnum.Subtraction, 5, 3, 2)]
        [TestCase(OperationEnum.Subtraction, -5, 3, -8)]
        [TestCase(OperationEnum.Subtraction, 0, 0, 0)]
        [TestCase(OperationEnum.Multiplication, 5, 3, 15)]
        [TestCase(OperationEnum.Multiplication, -5, 3, -15)]
        [TestCase(OperationEnum.Multiplication, 0, 5, 0)]
        [TestCase(OperationEnum.Division, 6, 3, 2)]
        [TestCase(OperationEnum.Division, -6, 3, -2)]
        [TestCase(OperationEnum.Division, 0, 5, 0)]
        public void Calculate_ShouldReturnCorrectResult(OperationEnum operation, double number1, double number2, double expected)
        {
            // Act
            double result = CalculatorHelper.Calculate(operation, number1, number2);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void Calculate_DivisionByZero_ShouldThrowDivideByZeroException()
        {
            // Arrange
            OperationEnum operation = OperationEnum.Division;
            double number1 = 5;
            double number2 = 0;

            // Act & Assert
            var ex = Assert.Throws<DivideByZeroException>(() => CalculatorHelper.Calculate(operation, number1, number2));
            Assert.That(ex.Message, Is.EqualTo("Nie można dzielić przez 0"));
        }

        [Test]
        public void Calculate_InvalidOperation_ShouldThrowArgumentException()
        {
            // Arrange
            OperationEnum invalidOperation = (OperationEnum)999;

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => CalculatorHelper.Calculate(invalidOperation, 1, 1));
            Assert.That(ex.Message, Does.StartWith("Nie znaleziono operacji:"));
            Assert.That(ex.ParamName, Is.EqualTo("operation"));
        }

        [TestCase(double.PositiveInfinity, 1)]
        [TestCase(1, double.PositiveInfinity)]
        [TestCase(double.NegativeInfinity, 1)]
        [TestCase(1, double.NaN)]
        public void Calculate_WithSpecialValues_ShouldHandleCorrectly(double number1, double number2)
        {
            // Act & Assert
            Assert.DoesNotThrow(() => CalculatorHelper.Calculate(OperationEnum.Add, number1, number2));
            Assert.DoesNotThrow(() => CalculatorHelper.Calculate(OperationEnum.Subtraction, number1, number2));
            Assert.DoesNotThrow(() => CalculatorHelper.Calculate(OperationEnum.Multiplication, number1, number2));
            Assert.DoesNotThrow(() => CalculatorHelper.Calculate(OperationEnum.Division, number1, number2));
        }

        [Test]
        public void Calculate_MultiplicationByZero_ShouldReturnZero()
        {
            Assert.That(CalculatorHelper.Calculate(OperationEnum.Multiplication, 5, 0), Is.EqualTo(0));
            Assert.That(CalculatorHelper.Calculate(OperationEnum.Multiplication, 0, 5), Is.EqualTo(0));
        }

        [Test]
        public void Calculate_DivisionOfZeroByNonZero_ShouldReturnZero()
        {
            Assert.That(CalculatorHelper.Calculate(OperationEnum.Division, 0, 5), Is.EqualTo(0));
        }

        [Test]
        public void Calculate_OperationsWithNaN_ShouldReturnNaN()
        {
            Assert.That(CalculatorHelper.Calculate(OperationEnum.Add, double.NaN, 5), Is.NaN);
            Assert.That(CalculatorHelper.Calculate(OperationEnum.Subtraction, 5, double.NaN), Is.NaN);
            Assert.That(CalculatorHelper.Calculate(OperationEnum.Multiplication, double.NaN, 5), Is.NaN);
            Assert.That(CalculatorHelper.Calculate(OperationEnum.Division, double.NaN, 5), Is.NaN);
        }

        [Test]
        public void Calculate_DivisionByInfinity_ShouldReturnZero()
        {
            Assert.That(CalculatorHelper.Calculate(OperationEnum.Division, 5, double.PositiveInfinity), Is.EqualTo(0));
            Assert.That(CalculatorHelper.Calculate(OperationEnum.Division, 5, double.NegativeInfinity), Is.EqualTo(0));
        }
    }
}
