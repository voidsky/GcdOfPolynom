using System;
using NUnit.Framework;

namespace MonomParse
{
    [TestFixture]
    public class MonomeTest
    {
        [TestCase(null, "")]
        [TestCase("", "")]
        [TestCase("x","x")]
        [TestCase("XX", "XX")]
        [TestCase(" 2XX ", "XX")]
        [TestCase(" -2XX^5 ", "XX")]
        [TestCase(" -2X X^-5 Y Y ", "X")]
        [TestCase("-5x^2", "x")]
        public void ParseMonomVariableResultsIn(string expression, string expectedResult)
        {
            Monome monom = new Monome(expression);
            string result = monom.Variable;
            Assert.AreEqual(expectedResult, result);
        }

        [TestCase("5", 5)]
        [TestCase(" 55 ", 55)]
        [TestCase(" -55 ", -55)]
        [TestCase("+1", 1)]
        [TestCase("-5x^2", -5)]
        public void ParseMonomCoefficientResultsIn(string expression, int expectedResult)
        {
            Monome monom = new Monome(expression);
            int result = monom.Coefficient;
            Assert.AreEqual(expectedResult, result);
        }

        [TestCase("-5x^2", 2)]
        [TestCase("-5x", 1)]
        [TestCase("x", 1)]
        [TestCase("", 1)]
        public void ParseMonomExponentResultsIn(string expression, int expectedResult)
        {
            Monome monom = new Monome(expression);
            int result = monom.Exponent;
            Assert.AreEqual(expectedResult, result);
        }
    }
}
