using System;
using System.Reflection;
using NUnit.Framework;

namespace MonomialParse
{
    [TestFixture]
    public class MonomialTest
    {
        [TestCase(null, "")]
        [TestCase("", "")]
        [TestCase("x", "x")]
        [TestCase(" XX", "XX")]
        [TestCase(" 2XX ", "XX")]
        [TestCase(" -2XX^5 ", "XX")]
        [TestCase(" -2X X^-5 Y Y ", "X")]
        [TestCase("-5x^2", "x")]
        [TestCase("-x^2", "x")]
        public void ParseMonomialVariableResultsIn(string expression, string expectedResult)
        {
            IExpressionParser parser = new ExpressionParser();
            Monomial monomial = new Monomial(expression, parser);
            string result = monomial.Variable;
            Assert.AreEqual(expectedResult, result);
        }

        [TestCase("5", 5)]
        [TestCase(" 55 ", 55)]
        [TestCase(" -55 ", -55)]
        [TestCase("+1", 1)]
        [TestCase("-5x^2", -5)]
        [TestCase("-x^2", -1)]
        public void ParseMonomialCoefficientResultsIn(string expression, int expectedResult)
        {
            IExpressionParser parser = new ExpressionParser();
            Monomial monomial = new Monomial(expression, parser);
            Assert.AreEqual(expectedResult, monomial.Coefficient);
        }

        [TestCase("-5x^2", 2)]
        [TestCase("-5x", 1)]
        [TestCase("x", 1)]
        [TestCase("", null)]
        [TestCase("-5x^-2", -2)]
        [TestCase("-x^2", 2)]
        public void ParseMonomialExponentResultsIn(string expression, int? expectedResult)
        {
            IExpressionParser parser = new ExpressionParser();
            Monomial monomial = new Monomial(expression, parser);
            int? result = monomial.Exponent;
            Assert.AreEqual(expectedResult, result);
        }

        [TestCase(5,"x",2,"5x^2")]
        [TestCase(-5, " yy ", -2, "-5yy^-2")]
        [TestCase(1, "x",1, "x")]
        [TestCase(null, "", null, "1")]
        [TestCase(-5, "", null, "-5")]
        [TestCase(null, "", -5, "1")]
        [TestCase(null, "x", null, "x")]
        public void CreateMonomialResultsIn(decimal? coefficient, string variable, int? exponent, string expression)
        {
            IExpressionParser parser = new ExpressionParser();
            Monomial monomial = new Monomial(coefficient, variable, exponent, parser);
            Assert.AreEqual(expression, monomial.Expression);
        }

        [TestCase("x^2","X^2",0)]
        [TestCase("x^3", "X^2", -1)]
        [TestCase("x^2", "X^3", 1)]
        [TestCase("5x^3", "X^3", -1)]
        [TestCase("x^3", "5X^3", 1)]
        [TestCase("1x^3", "X^3", 0)]
        public void CompareMonomialResultsIn(string firstExpression, string secondExpression, int result)
        {
            IExpressionParser parser = new ExpressionParser();
            Monomial firstMonomial = new Monomial(firstExpression, parser);
            Monomial secondMonomial = new Monomial(secondExpression, parser);
            Assert.AreEqual(result, firstMonomial.CompareTo(secondMonomial));
        }
    }
}
