using NUnit.Framework;
using Rekrutacja.Workers.Enums;
using Rekrutacja.Workers.Helper;
using System;

namespace Rekrutacja.Tests
{
    [TestFixture]
    public class FigureHelperTests
    {
        [Test]
        [TestCase(FigureEnum.Square, 5, 5, 25)]
        [TestCase(FigureEnum.Square, 10, 10, 100)]
        [TestCase(FigureEnum.Rectangle, 5, 3, 15)]
        [TestCase(FigureEnum.Rectangle, 10, 2, 20)]
        [TestCase(FigureEnum.Triangle, 6, 4, 12)]
        [TestCase(FigureEnum.Triangle, 5, 3, 7)]
        [TestCase(FigureEnum.Circle, 2, 0, 12)]
        [TestCase(FigureEnum.Circle, 1, 0, 3)]
        public void Calculate_ValidInputs_ShouldReturnCorrectArea(FigureEnum figure, double number1, double number2, int expected)
        {
            // Act
            int result = FigureHelper.Calculate(figure, number1, number2);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void Calculate_NegativeValues_ShouldThrowArgumentOutOfRangeException()
        {
            // Arrange
            FigureEnum figure = FigureEnum.Rectangle;
            double length = -5;
            double width = 3;

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => FigureHelper.Calculate(figure, length, width));
            Assert.That(ex.ParamName, Is.EqualTo("Długość: A, B"));
            Assert.That(ex.Message, Does.Contain("Wymiary figury muszą być dodatnie"));
        }

        [Test]
        public void Calculate_ZeroValues_ShouldThrowArgumentOutOfRangeException()
        {
            // Arrange
            FigureEnum figure = FigureEnum.Rectangle;
            double length = 0;
            double width = 5;

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => FigureHelper.Calculate(figure, length, width));
            Assert.That(ex.ParamName, Is.EqualTo("Długość: A, B"));
            Assert.That(ex.Message, Does.Contain("Wymiary figury muszą być dodatnie"));
        }

        [Test]
        public void Calculate_Square_UnequalSides_ShouldThrowArgumentException()
        {
            // Arrange
            FigureEnum figure = FigureEnum.Square;
            double side1 = 5;
            double side2 = 6;

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => FigureHelper.Calculate(figure, side1, side2));
            Assert.That(ex.Message, Does.Contain("Dla kwadratu obie długości muszą być równe"));
            Assert.That(ex.ParamName, Is.EqualTo("Długość: A, B"));
        }

        [Test]
        public void Calculate_InvalidFigure_ShouldThrowArgumentException()
        {
            // Arrange
            FigureEnum invalidFigure = (FigureEnum)999;
            double number1 = 5;
            double number2 = 5;

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => FigureHelper.Calculate(invalidFigure, number1, number2));
            Assert.That(ex.Message, Does.StartWith("Nie znaleziono figury"));
            Assert.That(ex.ParamName, Is.EqualTo("figure"));
        }

        [Test]
        public void Calculate_Circle_SecondParameterIgnored()
        {
            // Arrange
            FigureEnum figure = FigureEnum.Circle;
            double radius = 2;
            double ignoredValue = 100;

            // Act
            int result = FigureHelper.Calculate(figure, radius, ignoredValue);

            // Assert
            Assert.That(result, Is.EqualTo(12));
        }

        [Test]
        public void Calculate_LargeValues_ShouldNotOverflow()
        {
            // Arrange
            FigureEnum figure = FigureEnum.Rectangle;
            double length = Math.Sqrt(int.MaxValue);
            double width = Math.Sqrt(int.MaxValue);

            // Act
            int result = FigureHelper.Calculate(figure, length, width);

            // Assert
            Assert.That(result, Is.EqualTo(int.MaxValue));
        }
    }
}
