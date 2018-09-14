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
        public void ParseMonomialCoefficientResultsIn(string expression, int expectedResult)
        {
            IExpressionParser parser = new ExpressionParser();
            Monomial monomial = new Monomial(expression, parser);
            int? result = monomial.Coefficient;
            Assert.AreEqual(expectedResult, result);
        }

        [TestCase("-5x^2", 2)]
        [TestCase("-5x", 1)]
        [TestCase("x", 1)]
        [TestCase("", 1)]
        [TestCase("-5x^-2", -2)]
        public void ParseMonomialExponentResultsIn(string expression, int expectedResult)
        {
            IExpressionParser parser = new ExpressionParser();
            Monomial monomial = new Monomial(expression, parser);
            int? result = monomial.Exponent;
            Assert.AreEqual(expectedResult, result);
        }

        [TestCase(5,"x",2,"5x^2")]
        [TestCase(-5, " yy ", -2, "-5yy^-2")]
        [TestCase(1, "x",1, "1x^1")]
        [TestCase(null, "", null, "")]
        [TestCase(-5, "", null, "-5")]
        [TestCase(null, "", -5, "")]
        [TestCase(null, "x", null, "x")]
        public void CreateMonomialResultsIn(int? coefficient, string variable, int? exponent, string expression)
        {
            IExpressionParser parser = new ExpressionParser();
            Monomial monomial = new Monomial(coefficient, variable, exponent, parser);
            Assert.AreEqual(monomial.Expression, expression);
        }

        [Test]
        public void MonomialsAddResultsIn3x2()
        {
            IExpressionParser parser = new ExpressionParser();
            Monomial monomial = new Monomial(1,"x",2, parser);
            Monomial monomialToAdd = new Monomial(2,"x",2, parser);
            Monomial result = monomial.AddMonomialsWithSameVariable(monomialToAdd);
            Assert.AreEqual("3x^2", result.Expression);
        }

        [Test]
        public void MonomialsAddResultsIn3x()
        {
            IExpressionParser parser = new ExpressionParser();
            Monomial monomial = new Monomial(1, "x", null, parser);
            Monomial monomialToAdd = new Monomial(2, "x", null, parser);
            Monomial result = monomial.AddMonomialsWithSameVariable(monomialToAdd);
            Assert.AreEqual("3x", result.Expression);
        }

        [Test]
        public void MonomialsAddResultsIn3()
        {
            IExpressionParser parser = new ExpressionParser();
            Monomial monomial = new Monomial(1, "", null, parser);
            Monomial monomialToAdd = new Monomial(2, "", null, parser);
            Monomial result = monomial.AddMonomialsWithSameVariable(monomialToAdd);
            Assert.AreEqual("3", result.Expression);
        }

        [Test]
        public void MonomialsAddResultsInException()
        {
            IExpressionParser parser = new ExpressionParser();
            Monomial monomial = new Monomial(1, "x", 2, parser);
            Monomial monomialToAdd = new Monomial(2, "y", 2, parser);
            Assert.Throws<MonomialsCannotBeAddedException>(()=>
                monomial.AddMonomialsWithSameVariable(monomialToAdd));
        }

        [Test]
        public void MonomialsAddResultsInException2()
        {
            IExpressionParser parser = new ExpressionParser();
            Monomial monomial = new Monomial(1, "x", 2, parser);
            Monomial monomialToAdd = new Monomial(2, "x", 3, parser);
            Assert.Throws<MonomialsCannotBeAddedException>(() =>
                monomial.AddMonomialsWithSameVariable(monomialToAdd));
        }

    }
}
