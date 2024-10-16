using NUnit.Framework;
using Rekrutacja.Workers.Helper;
using System;

namespace Rekrutacja.Tests
{
    [TestFixture]
    public class ParserHelperTests
    {
        [Test]
        public void StringToInt_ValidPositiveNumber_ReturnsCorrectInt()
        {
            Assert.AreEqual(123, ParserHelper.StringToInt("123"));
        }

        [Test]
        public void StringToInt_ValidNegativeNumber_ReturnsCorrectInt()
        {
            Assert.AreEqual(-123, ParserHelper.StringToInt("-123"));
        }

        [Test]
        public void StringToInt_Zero_ReturnsZero()
        {
            Assert.AreEqual(0, ParserHelper.StringToInt("0"));
        }

        [Test]
        public void StringToInt_LargeNumber_ReturnsCorrectInt()
        {
            Assert.AreEqual(2147483647, ParserHelper.StringToInt("2147483647"));
        }

        [Test]
        public void StringToInt_SmallestNegativeNumber_ReturnsCorrectInt()
        {
            Assert.AreEqual(-2147483648, ParserHelper.StringToInt("-2147483648"));
        }

        [Test]
        public void StringToInt_EmptyString_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => ParserHelper.StringToInt(""));
        }

        [Test]
        public void StringToInt_Null_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => ParserHelper.StringToInt(null));
        }

        [Test]
        public void StringToInt_OnlyMinus_ThrowsFormatException()
        {
            Assert.Throws<FormatException>(() => ParserHelper.StringToInt("-"));
        }

        [Test]
        public void StringToInt_NonNumericCharacters_ThrowsFormatException()
        {
            Assert.Throws<FormatException>(() => ParserHelper.StringToInt("123a"));
        }

        [Test]
        public void StringToInt_DecimalNumber_ThrowsFormatException()
        {
            Assert.Throws<FormatException>(() => ParserHelper.StringToInt("123.45"));
        }

        [Test]
        public void StringToInt_MinusInMiddle_ThrowsFormatException()
        {
            Assert.Throws<FormatException>(() => ParserHelper.StringToInt("12-3"));
        }
    }
}
